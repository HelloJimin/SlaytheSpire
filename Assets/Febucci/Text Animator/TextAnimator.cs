#if UNITY_EDITOR || DEVELOPMENT_BUILD
#define CHECK_ERRORS //used to check text errors 
#endif

#if TA_Naninovel
#define INTEGRATE_NANINOVEL
#endif

using Febucci.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Febucci.UI
{
    [HelpURL("https://www.febucci.com/textanimator-docs")]
    [AddComponentMenu("Febucci/TextAnimator/TextAnimator")]
    [RequireComponent(typeof(TMP_Text)), DisallowMultipleComponent]
    public class TextAnimator : MonoBehaviour
    {
        public enum TimeScale
        {
            Scaled,
            Unscaled,
        }

        public TimeScale timeScale = TimeScale.Scaled;

        public float deltaTime { get; private set; }
        float timePassed;


#if INTEGRATE_NANINOVEL
        bool isNaninovelPresent;
        Naninovel.UI.IRevealableText reveablelText;
#endif


        bool hasParentCanvas;
        Canvas parentCanvas;

        private void Awake()
        {

            Canvas[] canvases = new Canvas[0];
            canvases = gameObject.GetComponentsInParent<Canvas>(true);

            if (canvases.Length > 0)
            {
                parentCanvas = canvases[0];
                hasParentCanvas = parentCanvas != null;
            }

#if INTEGRATE_NANINOVEL
            reveablelText = GetComponent<Naninovel.UI.IRevealableText>();
            isNaninovelPresent = reveablelText != null;
#endif
            TextUtilities.Initialize();

            //If we're checking text from TMPro, prevents its very first set text to appear for one frame and then disappear
            if (triggerAnimPlayerOnChange)
            {
                tmproText.renderMode = TextRenderFlags.DontRender;
            }
        }

        #region Consts
        //private const int barsSize = 3;
        //private const int verticesPerBar = 12; //bars are not used anymore

        public const int verticesPerChar = 4;

        public static bool IsTagLongEnough(string tag)
        {
            return tag.Length >= 3;
        }

        #endregion

        #region Types (Structs + Enums)

        [System.Serializable]
        class AppearancesContainer
        {
            [SerializeField]
            public string[] tags = new string[] { TAnimTags.ap_Size };  //starts with a size effect by default

            public AppearanceDefaultValues values = new AppearanceDefaultValues();
        }

        public struct CustomTag
        {
            public bool triggered { get; internal set; }
            public DefaultFeature defaultFeature;
            public CustomFeature customFeature;

            public int charIndex { get; internal set; }

            public List<string> parameters;

            internal int internalOrder;
        }

        #endregion

        #region Effects Access

        bool GetBehaviorClassFromTag(string tag, out BehaviorBase effectBase)
        {
            //Global Tags
            if (TAnimBuilder.TryGetGlobalBehaviorFromTag(tag, out effectBase))
            {
                return true;
            }

            //Local tags
            if (TAnimBuilder.TryGetEffectClassFromTag(localBehaviors, tag, out effectBase))
            {
                return true;
            }

            effectBase = default;
            return false;
        }

        bool GetAppearingClassFromTag(string tag, out AppearanceBase effectBase)
        {
            //Global Tags
            if (TAnimBuilder.TryGetGlobalAppearanceFromTag(tag, out effectBase))
            {
                return true;
            }

            //Local tags
            if (TAnimBuilder.TryGetEffectClassFromTag(localAppearances, tag, out effectBase))
            {
                return true;
            }

            effectBase = default;
            return false;
        }

        #endregion

        #region Variables

        TAnimPlayerBase _tAnimPlayer;
        TAnimPlayerBase tAnimPlayer
        {
            get
            {
                if (_tAnimPlayer != null)
                    return _tAnimPlayer;

#if UNITY_2019_2_OR_NEWER
                if(!TryGetComponent(out _tAnimPlayer))
                {
                    Debug.LogError($"Text Animator component is null on GameObject {gameObject.name}");
                }
#else
                _tAnimPlayer = GetComponent<TAnimPlayerBase>();
                Assert.IsNotNull(_tAnimPlayer, $"Text Animator Player component is null on GameObject {gameObject.name}");
#endif

                return _tAnimPlayer;
            }
        }


        #region Inspector

        [SerializeField, Tooltip("-If set to true, a TextAnimatorPlayer is triggered once TMPro text changes. Text can be shown dynamically (eg. typewriter) or not depending on its settings.\n-If false, the script sets and shows the entire text instantly.")]
        bool triggerAnimPlayerOnChange = false;

        [SerializeField]
        float effectIntensityMultiplier = 50;

        [UnityEngine.Serialization.FormerlySerializedAs("defaultAppearance"), SerializeField, Header("Text Appearance")]
        AppearancesContainer appearancesContainer = new AppearancesContainer();


        [SerializeField]
        BehaviorDefaultValues behaviorValues = new BehaviorDefaultValues();

        [SerializeField, Tooltip("True if you want effects to have the same intensities even if text is larger/smaller than default (example: when TMPro's AutoSize changes the size based on screen size)")]
        bool useDynamicScaling = false;
        [SerializeField, Tooltip("Used for scaling, represents the text's size where/when effects intensity behave like intended.")]
        float referenceFontSize = -1;

        #endregion

        #region Public Variables
        public delegate void MessageEVent(string message);
        public event MessageEVent onEvent;

        public bool hasFeatures { get; private set; }

        public string text { get => latestText; private set => latestText = value; }

        public bool allLettersShown => visibleCharacters >= tmproText.textInfo.characterCount;

        public TMP_CharacterInfo latestCharacterShown { get; private set; }

        #endregion

        #region Managament variables

        bool databaseBuilt = false;
        Dictionary<string, Type> localBehaviors = new Dictionary<string, Type>();
        Dictionary<string, Type> localAppearances = new Dictionary<string, Type>();

        string latestText;

        bool forceMeshRefresh;
        bool autoSize;
        Rect sourceRect;
        Color sourceColor;

        TMP_TextInfo textInfo;

        bool hasText = false;

        TMP_Text _tmproText;
        public TMP_Text tmproText
        {
            get
            {
                if (_tmproText != null)
                    return _tmproText;

#if UNITY_2019_2_OR_NEWER
                if(!TryGetComponent(out _tmproText))
                {
                    Debug.LogError("TextAnimator: TMproText component is null.");
                }
#else
                _tmproText = GetComponent<TMP_Text>();
                Assert.IsNotNull(tmproText, $"TextMeshPro component is null on Object {gameObject.name}");
#endif

                return _tmproText;
            }

            private set
            {
                _tmproText = value;
            }
        }

        int latestTriggeredEvent = 0;
        int latestTriggeredFeature = 0;

        int visibleCharacters;

        bool skipAppearanceEffects;

        CustomTag placeHolder = new CustomTag { defaultFeature = DefaultFeature.NotImplemented, customFeature = CustomFeature.NotImplemented };

        #endregion

        #region Text Elements

        Charact[] characters = new Charact[0];

        CustomTag[] customTags;

        BehaviorBase[] effects;

        AppearanceBase[] appearingEffects;

        EventMarker[] eventMarkers;

        int internalEventFeatureIndex;

        #endregion

        #endregion

#if CHECK_ERRORS
        void EDITOR_CompatibilityCheck(string text)
        {
            #region Text
            string textLower = text.ToLower();
            string errorsLog = "";

            //page
            if ((textLower.Contains("<page=")))
            {
                errorsLog += "- Tag <page> is not compatible\n";
            }

            if (errorsLog.Length > 0)
            {
                Debug.LogError($"TextAnimator: Given text not accepted [expand for more details]\n\nText:'{text}'\n\nErrors:\n{errorsLog}", this.gameObject);
            }
            #endregion
        }
#endif

        #region Public Methods

        /// <summary>
        /// Returns the next character to show
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGetNextCharacter(out TMP_CharacterInfo result)
        {
            if (visibleCharacters < textInfo.characterCount)
            {
                result = textInfo.characterInfo[visibleCharacters];
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Increases the visible Characters of the text.
        /// </summary>
        /// <returns>Returns the latest shown character</returns>
        public char IncreaseVisibleChars()
        {
            if (visibleCharacters > textInfo.characterCount || visibleCharacters < 0)
                return ' ';

            latestCharacterShown = textInfo.characterInfo[visibleCharacters];


            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (i >= visibleCharacters || !textInfo.characterInfo[i].isVisible)
                {
                    characters[i].data.passedTime = 0;
                }
            }

            TryTriggeringEvent(int.MaxValue); //Invokes all events that are after the current letter (but on the same TMPro index)

            visibleCharacters++;

            if (textInfo.characterInfo[visibleCharacters - 1].isVisible)
            { //might be a space or sprite

                return textInfo.characterInfo[visibleCharacters - 1].character;
            }

            return ' ';
        }

        /// <summary>
        /// Turns all characters visible (i.e. "a typewriter skip")
        /// </summary>
        /// <param name="skipAppearanceEffects">True if characters will be showed instantly, without any appearance effect</param>
        /// <param name="triggerRemainingEvents">True if we want to trigger all remaining events in the text</param>
        public void ShowAllCharacters(bool skipAppearanceEffects)
        {
            visibleCharacters = textInfo.characterCount;
            this.skipAppearanceEffects = skipAppearanceEffects;
        }

        public void TriggerRemainingEvents()
        {
            if (eventMarkers.Length <= 0)
                return;

            for (int i = latestTriggeredEvent; i < eventMarkers.Length; i++)
            {
                if (!eventMarkers[i].triggered)
                {
                    eventMarkers[i].triggered = true;
                    onEvent?.Invoke(eventMarkers[i].eventMessage);
                }
            }

            latestTriggeredEvent = eventMarkers.Length - 1;
        }

        void TryTriggeringEvent(int maxInternalOrder)
        {
            //Calls all events markers until the current shown visible character
            for (int i = latestTriggeredEvent; i < eventMarkers.Length; i++)
            {
                if (!eventMarkers[i].triggered && //current event must not be triggered already
                    eventMarkers[i].charIndex == textInfo.characterInfo[visibleCharacters].index && //Event is on index
                    eventMarkers[i].internalOrder < maxInternalOrder
                    )
                {
                    eventMarkers[i].triggered = true;
                    latestTriggeredEvent = i;
                    onEvent?.Invoke(eventMarkers[i].eventMessage);
                }
            }
        }


        /// <summary>
        /// Tries to get a feature in the current position of the text
        /// </summary>
        /// <param name="feature">Initialized feature</param>
        /// <returns>True if we have found one feature in the current text position</returns>
        public bool TryGetFeature(out CustomTag feature)
        {
            feature = placeHolder;

            if (visibleCharacters >= textInfo.characterCount) //avoids searching if text has ended
            {
                return false;
            }

            for (int i = latestTriggeredFeature; i < customTags.Length; i++)
            {
                if (customTags[i].charIndex == textInfo.characterInfo[visibleCharacters].index &&
                    !customTags[i].triggered)
                {
                    //tries triggering event, if it's written before function
                    TryTriggeringEvent(customTags[i].internalOrder);

                    customTags[i].triggered = true;
                    feature = customTags[i];
                    latestTriggeredFeature = i;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Formats the text, calculating the tags and vertexEffects regions
        /// </summary>
        /// <param name="text">original text, with tags etc.</param>
        /// <param name="hideText">hides the text</param>
        /// <returns>Formatted text (without tags)</returns>
        public void SyncText(string text, bool hideText)
        {
            _SyncText(text, hideText ? ShowTextMode.Hidden : ShowTextMode.Shown);
        }

        /// <summary>
        /// Forces TextAnimator to refresh the mesh at the end of the frame
        /// </summary>
        public void ForceMeshRefresh()
        {
            forceMeshRefresh = true;
        }

        #endregion

        #region Management Methods

        void BuildTagsDatabase()
        {

            if (databaseBuilt)
                return;

            TAnimBuilder.InitializeGlobalDatabase();

            databaseBuilt = true;

            //adds local behavior presets
            for (int i = 0; i < behaviorValues.presets.Length; i++)
            {
                TAnimBuilder.TryAddingPresetToDictionary(ref localBehaviors, behaviorValues.presets[i].effectTag, typeof(PresetBehavior));
            }

            //Adds local appearance presets
            for (int i = 0; i < appearancesContainer.values.presets.Length; i++)
            {
                TAnimBuilder.TryAddingPresetToDictionary(ref localAppearances, appearancesContainer.values.presets[i].effectTag, typeof(PresetAppearance));
            }

        }

        enum ShowTextMode : byte
        {
            Hidden = 0,
            Shown = 1,
            UserTyping = 2,
        }

        private void _SyncText(string text, ShowTextMode showTextMode)
        {
            internalEventFeatureIndex = 0;
            timePassed = 0; //resets time
            forceMeshRefresh = false;

            BuildTagsDatabase();

            //Prevents to calculate everything for an empty text
            if (text.Length <= 0)
            {
                hasText = false;
                text = string.Empty;
                _tmproText.text = string.Empty;
                tmproText.text = string.Empty;
                tmproText.ClearMesh();
                return;
            }

            skipAppearanceEffects = false;
            hasFeatures = false;

#if CHECK_ERRORS
            EDITOR_CompatibilityCheck(text);
#endif
            latestTriggeredEvent = 0;
            latestTriggeredFeature = 0;

            #region Const Tags
            const char charClosure = '/';
            const char charEvent = '?';
            const char charBehOpenTag = '<';
            const char charBehCloseTag = '>';
            const char charAppOpenTag = '{';
            const char charAppCloseTag = '}';
            #endregion

            List<BehaviorBase> behaviorEffects = new List<BehaviorBase>();
            List<AppearanceBase> appearanceEffects = new List<AppearanceBase>();

            List<EventMarker> eventMarkers = new List<EventMarker>();

            List<CustomTag> customTags = new List<CustomTag>();

            System.Text.StringBuilder realText = new System.Text.StringBuilder();

            #region Formats the text

            //Temporary variables
            string entireTag;
            string entireLoweredTag;
            int indexOfClosing;
            int indexOfNextOpening;
            bool hasFoundAppearance;

            for (int i = 0, realTextIndex = 0; i < text.Length; i++)
            {
                void InitializeEffectRegion<T>(ref T effect, string idTag) where T : EffectsBase
                {
                    //Sets the "common" values for the effect, no matter which type (Vertex or Color)
                    effect.charStartIndex = realTextIndex;
                    effect.charEndIndex = text.Length;
                    effect.effectTag = idTag;
                }

                hasFoundAppearance = text[i] == charAppOpenTag;

                if (text[i] == charBehOpenTag || hasFoundAppearance)
                {
                    char charToLookup = charBehCloseTag;

                    if (hasFoundAppearance)
                        charToLookup = charAppCloseTag;

                    indexOfNextOpening = text.IndexOf(text[i], i + 1);
                    indexOfClosing = text.IndexOf(charToLookup, i + 1);

                    //tag is valid
                    if (indexOfClosing >= 0 && //the tag ends somewhere
                            (
                                indexOfNextOpening > indexOfClosing || //next opening char is further from the closing (example, at first pos "<hello> <" is ok, "<<hello>" is wrong)
                                indexOfNextOpening < 0 //there isn't a next opening char
                            ))
                    {
                        //entire tag found, including < and >
                        entireTag = (text.Substring(i, indexOfClosing - i + 1));
                        entireLoweredTag = entireTag.ToLower();

                        if (hasFoundAppearance)
                        {
                            //first character of the tag, without the <
                            switch (entireLoweredTag[1])
                            {
                                default: //found a possible tag
                                    #region Start Tag

                                    entireLoweredTag = entireLoweredTag.Substring(1, entireLoweredTag.Length - 2);

                                    //found Appearance Tag
                                    if (GetAppearingClassFromTag(entireLoweredTag, out AppearanceBase effectBase))
                                    {
                                        InitializeEffectRegion(ref effectBase, entireLoweredTag);
                                        effectBase.SetDefaultValues(appearancesContainer.values);

                                        appearanceEffects.TryAddingNewRegion(effectBase);
                                    }
                                    else //no effects found
                                    {
                                        realText.Append(entireTag);
                                        realTextIndex += entireTag.Length;
                                        break;
                                    }

                                    #endregion
                                    break;

                                case charClosure: //found a '/', so we're closing a region

                                    //tries closing tags
                                    if (!appearanceEffects.CloseSingleOrAllEffects(entireLoweredTag.Substring(2, entireLoweredTag.Length - 3), realTextIndex))
                                    {
                                        //No effects found
                                        realText.Append(entireTag);
                                        realTextIndex += entireTag.Length;
                                        break;
                                    }

                                    break;
                            }

                        }
                        else
                        {

                            void TagNotFound()
                            {

                                //First part of the tag, "<ciao>" becomes "ciao"
                                string firstPartTag = entireLoweredTag.Substring(1, entireLoweredTag.Length - 2);

                                //Trims from the equal symbol. If it's "<ciao=3>" it becomes "ciao"
                                int trimmeredIndex = entireLoweredTag.IndexOf('=');
                                if (trimmeredIndex >= 0)
                                {
                                    firstPartTag = firstPartTag.Substring(0, trimmeredIndex - 1);
                                }
                                var baseT = TAnimBuilder.GetDefaultFeature(firstPartTag);
                                var customT = TAnimBuilder.GetCustomFeature(firstPartTag);

                                //checks if it has a functionality
                                if (baseT != DefaultFeature.NotImplemented || customT != CustomFeature.NotImplemented)
                                {
                                    hasFeatures = true;
                                    CustomTag newTag = default;

                                    newTag.customFeature = customT;
                                    newTag.defaultFeature = baseT;
                                    newTag.charIndex = realTextIndex;
                                    newTag.parameters = new List<string>();

                                    //the tag has also a part after the equal
                                    if (trimmeredIndex >= 0)
                                    {
                                        //creates its parameters

                                        string finalPartTag = entireLoweredTag.Substring(firstPartTag.Length + 2);

                                        finalPartTag = finalPartTag
                                            .Substring(0, finalPartTag.Length - 1);

                                        //Splits parameters
                                        newTag.parameters = finalPartTag.Split(',').ToList();
                                    }

                                    newTag.internalOrder = internalEventFeatureIndex;
                                    customTags.Add(newTag);


                                    internalEventFeatureIndex++; //increases internal events and features order
                                }
                                else //it may be a TMPro tag, pastes it to the text
                                {

                                    //No custom tag found, we paste the text into TMPro (adding < and >, since we removed it)
                                    //so it can be applied and formatted by TMPro itself (for example, when adding <align=right> or <color=red>, this
                                    //plugin "doesn't know" about them since, so it "sends" them to TMPro, which can format them properly).
                                    realText.Append(entireTag);
                                    realTextIndex += entireLoweredTag.Length; //increases the character index, since TMPro's tags are pasted into the text
                                }
                            }

                            //first character of the tag, without the <
                            switch (entireLoweredTag[1])
                            {
                                default: //found a possible tag
                                    #region Start Tag

                                    string trimmedTag = entireLoweredTag.Substring(1, entireLoweredTag.Length - 2);

                                    //All the tags inside the "< >" region (without the opening and ending chars, '<' and '>') separated by a space
                                    string[] tags = trimmedTag.Split(' ');

                                    //Adds a new region to the given list

                                    string firstTag = tags[0];

                                    //found Appearance Tag
                                    if (GetBehaviorClassFromTag(firstTag, out BehaviorBase effectBase))
                                    {
                                        InitializeEffectRegion(ref effectBase, firstTag);

                                        effectBase.SetDefaultValues(behaviorValues);

                                        behaviorEffects.TryAddingNewRegion(effectBase);
                                    }
                                    else //no effects found
                                    {
                                        TagNotFound();
                                        break;
                                    }

                                    #region Set Modifiers
                                    //Searches for modifiers inside the < > region (after the first tag, which we used to check the type of effect to add)
                                    for (int tagIndex = 1; tagIndex < tags.Length; tagIndex++)
                                    {
                                        int equalsIndex = tags[tagIndex].IndexOf('=');

                                        //we've found an "=" symbol, so we're setting the modifier
                                        if (equalsIndex >= 0)
                                        {
                                            //modifier name, from start to the equals symbol
                                            string modifierName = tags[tagIndex].Substring(0, equalsIndex);

                                            //Numeric value of the modifier (the part after the equal symbol)
                                            string modifierValueName = tags[tagIndex].Substring(equalsIndex + 1);
                                            modifierValueName = modifierValueName.Replace('.', ','); //replaces dots with commas

                                            //Applies the modifier to the effects region
                                            int effectIndex = behaviorEffects.GetIndexOfEffect(firstTag);
                                            if (effectIndex >= 0) //applies the effect if we've found the region
                                            {
                                                behaviorEffects[effectIndex].SetModifier(modifierName, modifierValueName);

#if UNITY_EDITOR
                                                behaviorEffects[effectIndex].EDITOR_RecordModifier(modifierName, modifierValueName);
#endif
                                            }
                                        }
                                    }
                                    #endregion

                                    #endregion
                                    break;

                                case charClosure: //found a '/', so we're closing a region
                                    #region Closure Tag

                                    bool closedRegion = false;

                                    //final, calculated removing both'<', '>' and '/'
                                    string closureTag = entireLoweredTag.Substring(2, entireLoweredTag.Length - 3);

                                    //Closes all the regions
                                    if (closureTag.Length == 0 || closureTag == charClosure.ToString()) //tag is <> or </>
                                    {

                                        //Closes ALL the region opened until now
                                        for (int k = 0; k < behaviorEffects.Count; k++)
                                        {
                                            closedRegion = behaviorEffects.CloseElement(k, realTextIndex);
                                        }
                                    }
                                    //Closes the current region
                                    else
                                    {
                                        closedRegion = behaviorEffects.CloseRegionNamed(closureTag, realTextIndex);
                                    }

                                    if (!closedRegion)
                                    {
                                        TagNotFound();
                                    }

                                    #endregion
                                    break;

                                case charEvent:
                                    #region Event Tag
                                    string eventTag = entireTag.Substring(2, entireTag.Length - 3); //tag, calculated removing both '<', '>' and '?'

                                    if (eventTag.Length > 0) //prevents from adding an empty callback
                                    {
                                        eventMarkers.Add(new EventMarker
                                        {
                                            charIndex = realTextIndex,
                                            eventMessage = eventTag,
                                            internalOrder = internalEventFeatureIndex,
                                        });

                                        internalEventFeatureIndex++; //increases internal events and features order
                                    }
                                    #endregion
                                    break;

                            }
                        }
                        //"skips" all the characters inside the tag, so we'll go back adding letters again
                        i = indexOfClosing;

                    }
                    else //pastes the tag opening/closing character, since the tag is not completed
                    {
                        realText.Append(text[i]);
                        realTextIndex++;
                    }
                }
                else
                {
                    realText.Append(text[i]);
                    realTextIndex++;
                }
            }
            #endregion

            #region Default appearing effects
            List<int> indexDefaultAppearances = new List<int>();

            //Default appearance effects
            for (int i = 0; i < appearancesContainer.tags.Length; i++)
            {
                if (appearancesContainer.tags[i].Length <= 0)
                {
                    continue;
                }

                if (GetAppearingClassFromTag(appearancesContainer.tags[i], out AppearanceBase effectBase))
                {
                    int existingIndex = appearanceEffects.GetIndexOfEffect(appearancesContainer.tags[i]);
                    if (existingIndex >= 0)
                    {
                        indexDefaultAppearances.Add(existingIndex);
                    }
                    else
                    {
                        effectBase.effectTag = appearancesContainer.tags[i];
                        appearanceEffects.Add(effectBase);
                        indexDefaultAppearances.Add(appearanceEffects.Count - 1);
                    }

                }
                else
                {
                    Debug.LogError($"TextAnimator: Appearance Tag '{appearancesContainer.tags[i]}' is not recognized.", this.gameObject);
                }
            }

            #endregion

            //Avoids rendering the text for half a frame
            tmproText.renderMode = TextRenderFlags.DontRender;


            //Applies the formatted to the component
            tmproText.text = realText.ToString();
            tmproText.ForceMeshUpdate();

            textInfo = tmproText.GetTextInfo(tmproText.text);

            #region Creates characters and gets info

            //Avoids rendering the entire text for another half frame
            tmproText.renderMode = TextRenderFlags.DontRender;

            #region Characters Setup
            {
                if (characters.Length < textInfo.characterCount)
                    Array.Resize(ref characters, textInfo.characterCount);

                List<int> effectsToApply = new List<int>(); //temporary

                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible)
                        continue;

                    //Calculates which effects are applied to this character

                    int[] GetEffectsDependency<T>(List<T> effects, ref bool hasEffects) where T : EffectsBase
                    {
                        effectsToApply.Clear();

                        //Checks if the character is inside a region of any effect, if yes we add a pointer to it
                        for (int l = 0; l < effects.Count; l++)
                        {
                            if (effects[l].IsCharInsideRegion(textInfo.characterInfo[i].index))
                            {
                                effectsToApply.Add(l);
                            }
                        }

                        //All the effects inside the given region that has to be applied to the character
                        hasEffects = effectsToApply.Count > 0;

                        return effectsToApply.ToArray();
                    }

                    //Assigns effects
                    characters[i].indexBehaviorEffects = GetEffectsDependency(behaviorEffects, ref characters[i].hasBehaviorEffects);
                    characters[i].indexAppearanceEffects = GetEffectsDependency(appearanceEffects, ref characters[i].hasAppearanceEffects);

                    //Assigns default appearances
                    if (indexDefaultAppearances.Count > 0
                        && (!characters[i].hasAppearanceEffects || characters[i].indexAppearanceEffects.Length <= 0))
                    {
                        characters[i].hasAppearanceEffects = true;
                        characters[i].indexAppearanceEffects = indexDefaultAppearances.ToArray();
                    }

                    #region Sources and data

                    //Creates sources and data arrays only the first time
                    if (characters[i].data.vertices is null)
                    {
                        characters[i].sources.vertices = new Vector3[verticesPerChar];
                        characters[i].sources.colors = new Color32[verticesPerChar];

                        characters[i].data.vertices = new Vector3[verticesPerChar];
                        characters[i].data.colors = new Color32[verticesPerChar];
                    }
                    else
                    {
                        //Resets data arrays, since it may have already been populated by past texts
                        for (byte k = 0; k < verticesPerChar; k++)
                        {
                            characters[i].data.vertices[k] = Vector3.zero;
                            characters[i].data.colors[k] = Color.clear; //TODO Color32.clear
                        }
                    }

                    //Copies source data from the mesh info
                    for (byte k = 0; k < verticesPerChar; k++)
                    {
                        //vertices
                        characters[i].sources.vertices[k] = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices[textInfo.characterInfo[i].vertexIndex + k];

                        //colors
                        characters[i].sources.colors[k] = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32[textInfo.characterInfo[i].vertexIndex + k];
                    }

                    #endregion
                }
            }

            #endregion

            #region Effects and Features Initialization

            //Copies the effects
            this.effects = behaviorEffects.ToArray();
            this.appearingEffects = appearanceEffects.ToArray();
            this.customTags = customTags.ToArray();

            for (int i = 0; i < this.appearingEffects.Length; i++)
            {
                this.appearingEffects[i].SetDefaultValues(appearancesContainer.values);
            }

            SetupEffectsIntensity();



            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].Initialize(characters.Length);
            }

            for (int i = 0; i < appearingEffects.Length; i++)
            {
                appearingEffects[i].Initialize(characters.Length);
            }

            #endregion

            #endregion

            this.eventMarkers = eventMarkers.ToArray();


            this.text = tmproText.text;

            void HideAllCharacters()
            {
                visibleCharacters = 0;

                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    if (i >= visibleCharacters)
                    {
                        characters[i].data.passedTime = 0;
                    }
                }

                if (visibleCharacters <= 0 && characters.Length > 0)
                {
                    characters[0].data.passedTime = 0;
                }

            }

            void ShowAllCharacters()
            {
                visibleCharacters = textInfo.characterCount;

                //resets letters time
                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    characters[i].data.passedTime = 0;
                }
            }

            switch (showTextMode)
            {
                case ShowTextMode.Hidden:
                    HideAllCharacters();
                    break;
                case ShowTextMode.Shown:
                    ShowAllCharacters();
                    break;
                case ShowTextMode.UserTyping:
                    visibleCharacters = textInfo.characterCount;

#if INTEGRATE_NANINOVEL
                    //Hides characters based on naninovel's progress 
                    for (int i = 0; i < characters.Length; i++)
                    {
                        if (i >= Mathf.CeilToInt(Mathf.Clamp01(reveablelText.RevealProgress) * textInfo.characterCount))
                        {
                            characters[i].data.passedTime = 0;
                        }
                    }
#endif

                    if (visibleCharacters - 1 < characters.Length && visibleCharacters - 1 >= 0)
                        characters[visibleCharacters - 1].data.passedTime = 0; //user is typing, the latest letter has time reset
                    break;
            }

            hasText = realText.Length > 0;
            autoSize = tmproText.enableAutoSizing;

            //Avoids the next tmpro inserted text to be rendered for half a frame
            if (triggerAnimPlayerOnChange)
            {
                tmproText.renderMode = TextRenderFlags.DontRender;
            }
        }

        #endregion

        /// <summary>
        /// Assigns intensity multiplier and effect values/parameters to effects
        /// </summary>
        void SetupEffectsIntensity()
        {
            float intensity = effectIntensityMultiplier;

            if (useDynamicScaling)
            {
                //multiplies by font size
                intensity *= tmproText.fontSize / referenceFontSize;
            }

            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].SetEffectIntensity(intensity);
            }

            for (int i = 0; i < appearingEffects.Length; i++)
            {
                appearingEffects[i].SetEffectIntensity(intensity);
            }

        }

        void CopyMeshSources()
        {
            forceMeshRefresh = false;
            autoSize = tmproText.enableAutoSizing;
            sourceRect = tmproText.rectTransform.rect;
            sourceColor = tmproText.color;

            SetupEffectsIntensity();

            //Updates the characters sources
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                //Updates vertices
                for (byte k = 0; k < verticesPerChar; k++)
                {
                    characters[i].sources.vertices[k] = textInfo.meshInfo[tmproText.textInfo.characterInfo[i].materialReferenceIndex].vertices[tmproText.textInfo.characterInfo[i].vertexIndex + k];
                }

                //Updates colors
                for (byte k = 0; k < verticesPerChar; k++)
                {
                    characters[i].sources.colors[k] = textInfo.meshInfo[tmproText.textInfo.characterInfo[i].materialReferenceIndex].colors32[tmproText.textInfo.characterInfo[i].vertexIndex + k];
                }
            }
        }

        /// <summary>
        /// Applies the changes to the text component
        /// </summary>
        void UpdateMesh()
        {
            //Updates the mesh
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                //Avoids updating if we're on an invisible character, like a spacebar
                //Do not switch this with "i<visibleCharacters", since the plugin has to update not yet visible characters
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                //Updates vertices
                for (byte k = 0; k < verticesPerChar; k++)
                {
                    textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices[textInfo.characterInfo[i].vertexIndex + k] = characters[i].data.vertices[k];
                }

                //Updates colors
                for (byte k = 0; k < verticesPerChar; k++)
                {
                    textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32[textInfo.characterInfo[i].vertexIndex + k] = characters[i].data.colors[k];
                }
            }

            tmproText.UpdateVertexData();
        }

        private void Update()
        {
            //TMPRO's text changed, setting the text again
            if (!tmproText.text.Equals(text))
            {
                if (hasParentCanvas && !parentCanvas.isActiveAndEnabled)
                    return;

                //trigers anim player
                if (triggerAnimPlayerOnChange && tAnimPlayer != null)
                {

#if TA_NoTempFix
                    tAnimPlayer.ShowText(tmproText.text);
#else

                    //temp fix, opening and closing this TMPro tag (which won't be showed in the text, acting like they aren't there) because otherwise
                    //there isn't any way to trigger that the text has changed, if it's actually the same as the previous one.

                    if (tmproText.text.Length <= 0) //forces clearing the mesh during the tempFix, without the <noparse> tags
                        tAnimPlayer.ShowText("");
                    else
                        tAnimPlayer.ShowText($"<noparse></noparse>{tmproText.text}");
#endif

                }
                else //user is typing from TMPro
                {
                    _SyncText(tmproText.text, ShowTextMode.UserTyping);
                }

                return;
            }

            if (!hasText)
                return;

            //Manages Time
            deltaTime = timeScale == TimeScale.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;

            //To avoid possible desync errors etc., effects can't be played backwards. 
            if (deltaTime < 0)
                deltaTime = 0;

            timePassed += deltaTime;

            #region Effects Calculation

            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].SetTimeValues(timePassed, deltaTime);
                effects[i].Calculate();
            }

            for (int i = 0; i < appearingEffects.Length; i++)
            {
                appearingEffects[i].Calculate();
            }
            #endregion

            for (int i = 0; i < textInfo.characterCount; i++)
            {

#if INTEGRATE_NANINOVEL
                //If we're integrating naninovels, shows characters based on its reveal component
                if (isNaninovelPresent)
                {
                    if (reveablelText.RevealProgress < (float)i / textInfo.characterCount)
                        continue;
                }
#endif

                //applies effects only if the character is visible
                if (i < visibleCharacters && textInfo.characterInfo[i].isVisible)
                {
                    characters[i].data.passedTime += deltaTime;

                    characters[i].ResetColors();
                    characters[i].ResetVertices();

                    //behaviors
                    if (characters[i].hasBehaviorEffects)
                    {
                        for (int l = 0; l < characters[i].indexBehaviorEffects.Length; l++)
                        {
                            effects[
                                characters[i].indexBehaviorEffects[l] //indexes of the effect to apply
                                ].ApplyEffect(ref characters[i].data, i);
                        }
                    }

                    //appearances
                    if (!skipAppearanceEffects && characters[i].hasAppearanceEffects)
                    {
                        for (int l = 0; l < characters[i].indexAppearanceEffects.Length; l++)
                        {

                            if (appearingEffects[characters[i].indexAppearanceEffects[l]].CanShowAppearanceOn(characters[i].data.passedTime))
                            {
                                appearingEffects[
                                    characters[i].indexAppearanceEffects[l]
                                    ].ApplyEffect(ref characters[i].data, i);
                            }
                        }
                    }
                }
            }

            UpdateMesh();

            //TMPro's component changed, recalculating mesh
            //P.S. Must be placed after everything else.
            if (tmproText.havePropertiesChanged ||
                forceMeshRefresh ||
                //changing below properties seem to not trigger 'havePropertiesChanged', so we're checking them manually
                tmproText.enableAutoSizing != autoSize ||
                tmproText.rectTransform.rect != sourceRect ||
                tmproText.color != sourceColor)
            {
                tmproText.ForceMeshUpdate();
                CopyMeshSources();
            }

        }

#if UNITY_EDITOR

        private void OnEnable()
        {
            TAnim_EditorHelper.onChangesApplied += EDITORONLY_ResetEffects;
        }

        private void OnDisable()
        {
            TAnim_EditorHelper.onChangesApplied -= EDITORONLY_ResetEffects;
        }

        void EDITORONLY_ResetEffects()
        {
            if (!Application.isPlaying)
                return;

            if (effects != null && appearingEffects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    effects[i].SetDefaultValues(behaviorValues);
                }

                for (int i = 0; i < appearingEffects.Length; i++)
                {
                    appearingEffects[i].SetDefaultValues(appearancesContainer.values);
                }

                SetupEffectsIntensity();

                for (int i = 0; i < effects.Length; i++)
                {
                    effects[i].EDITOR_ApplyModifiers();
                }
            }
        }
#endif
    }
}