using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Febucci.UI.Core
{
    [System.Serializable]
    public class CharacterEvent : UnityEvent<char> { }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextAnimator))]
    public abstract class TAnimPlayerBase : MonoBehaviour
    {
        public delegate void VoidCallback();

        TextAnimator _textAnimator;
        public TextAnimator textAnimator
        {
            get
            {

                if (_textAnimator != null)
                    return _textAnimator;

#if UNITY_2019_2_OR_NEWER
                if(!TryGetComponent(out _textAnimator))
                {
                    Debug.LogError($"TextAnimator: Text Animator component is null on GameObject {gameObject.name}");
                }
#else
                _textAnimator = GetComponent<TextAnimator>();
                Assert.IsNotNull(_textAnimator, $"Text Animator component is null on GameObject {gameObject.name}");
#endif

                return _textAnimator;
            }
        }

        protected bool isBaseInsideRoutine => isInsideRoutine;
        bool isInsideRoutine = false;

        protected bool wantsToSkip = false;
        string textToShow = string.Empty;

        [Header("Base")]
        [Tooltip("True = shows the text dynamically")]
        [SerializeField] public bool useTypeWriter = true;
        [Header("Typewriter Skip")]
        [SerializeField] bool canSkipTypewriter = true;
        [SerializeField] bool hideAppearancesOnSkip = false;
        [SerializeField, Tooltip("True = plays all remaining events once the typewriter has been skipped")] bool triggerEventsOnSkip = false;

        /// <summary>
        /// Typewriter's speed (acts like a multiplier)
        /// You can change this value or invoke SetTypewriterSpeed
        /// </summary>
        protected float typewriterPlayerSpeed = 1;

        #region Events
        [Header("Events")]
        /// <summary>
        /// Called once the text is completely shown (if typewriter is set to true, this event is called after its end)
        /// </summary>
        public UnityEvent onTextShowed;

        /// <summary>
        /// Called only once the typewriter has completed showing the text
        /// </summary>
        [System.Obsolete("This event will be removed from the next version. Please use the 'onTextShowed' method instead")]
        public event VoidCallback onTypeWriterEnded;

        /// <summary>
        /// Called once the typewriter starts showing text.
        /// </summary>
        public UnityEvent onTypewriterStart;

        /// <summary>
        /// Called once a character has been shown
        /// </summary>
        public CharacterEvent onCharacterVisible;

        #endregion

        #region Routines

        /// <summary>
        /// Typewriter effect
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private IEnumerator ShowVisibleCharacters()
        {
            if (!textAnimator.allLettersShown)
            {
                isInsideRoutine = true;
                wantsToSkip = false;
                float t;

                onTypewriterStart?.Invoke();

#pragma warning disable 0618 //temp obsolete warning disable or any user that did not implement it will have a warning as well
                OnTypeWriterStart();
#pragma warning restore 0618

                IEnumerator WaitTime(float time)
                {
                    if (time > 0)
                    {
                        t = 0;
                        while (t <= time)
                        {
                            t += textAnimator.deltaTime;
                            yield return null;
                        }
                    }
                }

                float timeToWait;
                char characterShown = ' ';

                typewriterPlayerSpeed = 1;

                float typewriterTagsSpeed = 1;

                bool HasSkipped()
                {
                    return canSkipTypewriter && wantsToSkip;
                }

                //Shows character by character until all are shown
                while (!textAnimator.allLettersShown)
                {

                    //searches for features
                    if (textAnimator.hasFeatures)
                    {
                        //loops until features ended (there could be multiple ones in the same text position, example: when two tags are next to eachother without spaces
                        while (textAnimator.TryGetFeature(out TextAnimator.CustomTag feature))
                        {
                            //Default features
                            switch (feature.defaultFeature)
                            {
                                case DefaultFeature.WaitFor:
                                    float waitTime;
                                    FormatUtils.TryGetFloat(feature.parameters, 0, 1f, out waitTime);
                                    yield return WaitTime(waitTime);
                                    break;

                                case DefaultFeature.WaitInput:
                                    yield return WaitInput();
                                    break;

                                case DefaultFeature.TypewriterSpeedMult:
                                    FormatUtils.TryGetFloat(feature.parameters, 0, 1, out typewriterTagsSpeed);

                                    //clamps speed (time cannot go backwards!)
                                    if (typewriterTagsSpeed <= 0)
                                    {
                                        typewriterTagsSpeed = 0.001f;
                                    }

                                    break;
                            }

                            //Custom features
                            if (feature.customFeature != CustomFeature.NotImplemented)
                            {
                                yield return DoCustomFeature(feature.customFeature, feature.parameters);
                            }
                        }
                    }

                    characterShown = textAnimator.IncreaseVisibleChars();

                    t = 0;
                    timeToWait = WaitTimeOf(//get the wait time based
                        characterShown //increases characters
                        );


                    //triggers event unless it's a space
                    if (characterShown != ' ')
                    {
                        onCharacterVisible?.Invoke(characterShown);
                    }

                    while (t <= timeToWait && !HasSkipped())
                    {
                        UpdateTypeWriterInput();

                        t += textAnimator.deltaTime * typewriterPlayerSpeed * typewriterTagsSpeed;

                        yield return null;
                    }


                    //Skips typewriter
                    if (HasSkipped())
                    {
                        textAnimator.ShowAllCharacters(hideAppearancesOnSkip);

                        if (triggerEventsOnSkip)
                        {
                            textAnimator.TriggerRemainingEvents();
                        }

                        break;
                    }

                }

                // triggers the events at the end of the text
                // 
                // the typewriter is arrived here without skipping
                // meaning that all events were triggered and we only
                // have to fire the ones at the very end
                // (outside tmpro's characters count length)
                if (!canSkipTypewriter || !wantsToSkip)
                {
                    textAnimator.TriggerRemainingEvents();
                }
                
                isInsideRoutine = false;

                textToShow = string.Empty; //text has been showed, no need to store it now

                onTextShowed?.Invoke();

#pragma warning disable 0618 //temp obsolete warning disable or any user that did not implement it will have a warning as well
                onTypeWriterEnded?.Invoke();
#pragma warning restore 0618
            }
        }

        /// <summary>
        /// Waits for input in order to continue showing text.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator WaitInput();

        protected virtual IEnumerator DoCustomFeature(CustomFeature featureType, List<string> parameters)
        {
            throw new System.NotImplementedException($"TextAnimator: Custom Feature not implemented with type: {featureType}. If you did implement it, please do not call the base method from your overridden one.");
        }

        #endregion

        /// <summary>
        /// Lets us wait for X time based on certain characters
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected abstract float WaitTimeOf(char character);

        protected virtual void UpdateTypeWriterInput()
        {

        }

        [System.Obsolete("This method will be removed from the next versions. Please use the 'OnTypewriterStart' method instead")]
        protected virtual void OnTypeWriterStart()
        {

        }

        public void StopShowingText()
        {
            //Stops only if we're inside the routine
            if (isInsideRoutine)
            {
                isInsideRoutine = false;
                StopAllCoroutines();
            }

            textToShow = string.Empty;
        }

        public void ShowText(string text)
        {
            StopShowingText();

            if (string.IsNullOrEmpty(text))
            {
                textToShow = string.Empty;
                textAnimator.SyncText(string.Empty, true);
                return;
            }

            textToShow = text;

            TryShowingText();
        }


        /// <summary>
        /// Skips the typewriter animation (if it's currently showing)
        /// </summary>
        public void SkipTypewriter()
        {
            wantsToSkip = true;
        }

        /// <summary>
        /// Sets the internal typewriter speed multiplier.
        /// </summary>
        /// <param name="value"></param>
        public void SetTypewriterSpeed(float value)
        {
            typewriterPlayerSpeed = Mathf.Clamp(value, .05f, value);
        }

        void TryShowingText()
        {
            if (!string.IsNullOrEmpty(textToShow))
            {
                wantsToSkip = false;

                textAnimator.SyncText(textToShow, useTypeWriter);

                if (!useTypeWriter)
                {
                    onTextShowed?.Invoke();
                }

                if (useTypeWriter && gameObject.activeInHierarchy)
                {
                    StartCoroutine(ShowVisibleCharacters());
                }
            }
        }

        protected virtual void OnDisable()
        {
            isInsideRoutine = false;
        }

        //Resumes/Shows text once the gameobject is active
        protected virtual void OnEnable()
        {
            if (!isInsideRoutine && useTypeWriter)
            {
                StartCoroutine(ShowVisibleCharacters());
            }
        }
    }

}