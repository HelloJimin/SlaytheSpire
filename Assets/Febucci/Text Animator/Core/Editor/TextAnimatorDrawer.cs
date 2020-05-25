using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Febucci.UI.Core
{
    [CustomEditor(typeof(TextAnimator))]
    public class TextAnimatorDrawer : Editor
    {
        const string alertTextSizeDep = "This effect's strength changes with different sizes and fonts.";

        static readonly Color expandedColor = new Color(1, 1, 1);
        static readonly Color errorColor = new Color(1, .6f, .6f);
        static readonly Color selectedShowColor = new Color(.7f, 1, .7f);
        static readonly Color notExpandedColor = Color.gray * 1.5f;
        static readonly Color sectionsColor = new Color(.95f, .95f, .95f);

        static string availableAppBuiltinTagsLongText;

        #region Structs
        struct Effect
        {
            bool show;
            bool dependant; //if effect changes based on size
            string effectName;
            public string effectTag { get; private set; }

            List<SerializedProperty> properties;

            public Effect(string effectName, string effectTag, bool dependant, SerializedProperty parentProperty, params string[] names)
            {
                this.dependant = dependant;
                this.effectName = "-> " + (dependant ? "[!] " : "") + effectName + ", <" + effectTag + '>';
                this.effectTag = effectTag;
                show = false;

                properties = new List<SerializedProperty>();
                for (int i = 0; i < names.Length; i++)
                {
                    properties.Add(parentProperty.FindPropertyRelative(names[i]));
                }
            }

            public void Show()
            {
                if (show)
                    GUI.backgroundColor = expandedColor;
                else
                    GUI.backgroundColor = notExpandedColor;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if (GUILayout.Button(effectName, show ? EditorStyles.boldLabel : EditorStyles.label))
                    show = !show;

                GUI.backgroundColor = Color.white;

                if (show)
                {
                    if (dependant)
                    {
                        EditorGUILayout.LabelField(alertTextSizeDep, EditorStyles.centeredGreyMiniLabel);
                    }

                    for (int i = 0; i < properties.Count; i++)
                    {
                        EditorGUILayout.PropertyField(properties[i]);
                    }
                }

                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// Class that draws the component's default appearance effects
        /// </summary>
        class AppearanceDefaultEffects
        {
            GUIContent tagsContainerLabel;

            SerializedProperty tagsContainer;
            SerializedProperty customs;

            bool showBuiltIn;

            Effect appSize;
            Effect appFade;
            Effect appVertExp;
            Effect appHoriExp;
            Effect appDiagExp;
            Effect appOffset;
            Effect appRot;

            public AppearanceDefaultEffects(SerializedProperty preset)
            {
                tagsContainerLabel = new GUIContent("Default Tags");
                this.tagsContainer = preset.FindPropertyRelative("tags");
                var values = preset.FindPropertyRelative("values");
                var defaults = values.FindPropertyRelative("defaults");
                this.customs = values.FindPropertyRelative("customs");
                this.showBuiltIn = false;
                appSize = new Effect(
                    "Size",
                    TAnimTags.ap_Size,
                    false,
                    defaults,
                    "sizeDuration",
                    "sizeAmplitude"
                    );

                appFade = new Effect(
                    "Fade",
                    TAnimTags.ap_Fade,
                    false,
                    defaults,
                    "fadeDuration"
                    );

                appVertExp = new Effect(
                    "Vertical Expand",
                    TAnimTags.ap_VertExp,
                    false,
                    defaults,
                    "verticalExpandDuration",
                    "verticalFromBottom"
                    );

                appHoriExp = new Effect(
                    "Horizontal Expand",
                    TAnimTags.ap_HoriExp,
                    false,
                    defaults,
                    "horizontalExpandDuration",
                    "horizontalExpandStart"
                    );

                appDiagExp = new Effect(
                    "Diagonal Expand",
                    TAnimTags.ap_DiagExp,
                    false,
                    defaults,
                    "diagonalExpandDuration",
                    "diagonalFromBttmLeft"
                    );

                appOffset = new Effect(
                    "Offset",
                    TAnimTags.ap_Offset,
                    true,
                    defaults,
                    "offsetDir",
                    "offsetDuration",
                    "offsetAmplitude"
                    );

                appRot = new Effect(
                    "Rotation",
                    TAnimTags.ap_Rot,
                    false,
                    defaults,
                    "rotationDuration",
                    "rotationStartAngle"
                    );
            }

            public void ShowCustom()
            {
                ShowCustomEffectsValues(customs);
            }

            public void Show()
            {
                //avaiable tags
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    EditorGUILayout.LabelField($"Available Built-in tags: {availableAppBuiltinTagsLongText}", EditorStyles.wordWrappedMiniLabel);

                    EditorGUILayout.LabelField("Default Appearance Effects", EditorStyles.miniBoldLabel);

                    if (Application.isPlaying)
                        GUI.enabled = false;

                    EditorGUILayout.PropertyField(tagsContainer, tagsContainerLabel, true);
                    GUI.enabled = false;
                    EditorGUILayout.LabelField("Write one tag for each element. In case it is not specified via appearance tags, the animator will apply the above effects to letters.", EditorStyles.wordWrappedMiniLabel);
                    GUI.enabled = true;
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();

                //show default toggle
                GUI.backgroundColor = sectionsColor;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;

                showBuiltIn = EditorGUILayout.BeginToggleGroup("Edit built-in effects", showBuiltIn);

                if (showBuiltIn)
                {

                    GUI.backgroundColor = sectionsColor;
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    appOffset.Show();
                    appSize.Show();
                    appFade.Show();
                    appVertExp.Show();
                    appHoriExp.Show();
                    appDiagExp.Show();
                    appRot.Show();

                    EditorGUILayout.EndVertical();

                }

                EditorGUILayout.EndToggleGroup();


                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// Draws default behavior effects
        /// </summary>
        class BehaviorDefaultEffects
        {
            bool showBuiltIn;
            Effect behWiggle;
            Effect behWave;
            Effect behRotation;
            Effect behSwing;
            Effect behShake;
            Effect behSize;
            Effect behSlide;
            Effect behBounce;
            Effect behRainbow;
            Effect behFade;

            public BehaviorDefaultEffects(SerializedProperty defaultValues)
            {

                behWiggle = new Effect(
                    "Wiggle",
                    TAnimTags.bh_Wiggle,
                    true,
                    defaultValues,
                    "wiggleAmplitude",
                    "wiggleFrequency"
                    ); ;

                behWave = new Effect(
                    "Wave",
                    TAnimTags.bh_Wave,
                    true,
                    defaultValues,
                    "waveFrequency",
                    "waveAmplitude",
                    "waveWaveSize"
                    ); ;

                behRotation = new Effect(
                    "Rotation",
                    TAnimTags.bh_Rot,
                    false,
                    defaultValues,
                    "angleSpeed",
                    "angleDiffBetweenChars"
                    ); ;

                behSwing = new Effect(
                    "Swing",
                    TAnimTags.bh_Swing,
                    false,
                    defaultValues,
                    "swingAmplitude",
                    "swingFrequency",
                    "swingWaveSize"
                    ); ;

                behShake = new Effect(
                    "Shake",
                    TAnimTags.bh_Shake,
                    true,
                    defaultValues,
                    "shakeStrength",
                    "shakeDelay"
                    ); ;

                behSize = new Effect(
                    "Increase",
                    TAnimTags.bh_Incr,
                    false,
                    defaultValues,
                    "sizeAmplitude",
                    "sizeFrequency",
                    "sizeWaveSize"
                    ); ;

                behSlide = new Effect(
                    "Slide",
                    TAnimTags.bh_Slide,
                    true,
                    defaultValues,
                    "slideAmplitude",
                    "slideFrequency",
                    "slideWaveSize"
                    ); ;

                behBounce = new Effect(
                    "Bounce",
                    TAnimTags.bh_Bounce,
                    true,
                    defaultValues,
                    "bounceAmplitude",
                    "bounceFrequency",
                    "bounceWaveSize"
                    ); ;

                behRainbow = new Effect(
                    "Rainbow",
                    TAnimTags.bh_Rainb,
                    false,
                    defaultValues,
                    "hueShiftSpeed",
                    "hueShiftWaveSize"
                    ); ;

                behFade = new Effect(
                    "Fade",
                    TAnimTags.bh_Fade,
                    false,
                    defaultValues,
                    "fadeDelay"
                    ); ;
            }

            public void Show()
            {
                GUI.backgroundColor = sectionsColor;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;

                showBuiltIn = EditorGUILayout.BeginToggleGroup("Edit built-in effects", showBuiltIn);

                if (showBuiltIn)
                {
                    behWiggle.Show();
                    behShake.Show();
                    behWave.Show();
                    behSlide.Show();
                    behBounce.Show();

                    behRotation.Show();
                    behSwing.Show();
                    behSize.Show();
                    behRainbow.Show();
                    behFade.Show();
                }
                EditorGUILayout.EndToggleGroup();


                EditorGUILayout.EndVertical();

            }
        }

        /// <summary>
        /// Manages a single preset (behavior or appearance) 
        /// </summary>

        internal class UserPresetDrawer
        {
            public bool show;
            public bool wantsToRemove;
            bool isAppearance;
            public string getName => isAppearance ? ('{' + effectTag.stringValue + '}') : ('<' + effectTag.stringValue + '>');

            SerializedProperty effectTag;

            EmissionCurveDrawer emission;

            FloatCurveDrawer movementX;
            FloatCurveDrawer movementY;
            FloatCurveDrawer movementZ;

            FloatCurveDrawer scaleX;
            FloatCurveDrawer scaleY;

            FloatCurveDrawer rotX;
            FloatCurveDrawer rotY;
            FloatCurveDrawer rotZ;

            ColorCurveDrawer color;

            public UserPresetDrawer(SerializedProperty parent, bool isAppearance)
            {
                effectTag = parent.FindPropertyRelative("effectTag");

                this.isAppearance = isAppearance;
                this.show = false;
                this.wantsToRemove = false;

                if (!isAppearance)
                {
                    emission = new EmissionCurveDrawer(parent.FindPropertyRelative("emission"));
                }

                movementX = new FloatCurveDrawer(parent.FindPropertyRelative("movementX"), "Movement X", true, isAppearance, Color.red);
                movementY = new FloatCurveDrawer(parent.FindPropertyRelative("movementY"), "Movement Y", true, isAppearance, Color.green);
                movementZ = new FloatCurveDrawer(parent.FindPropertyRelative("movementZ"), "Movement Z", true, isAppearance, Color.cyan);

                scaleX = new FloatCurveDrawer(parent.FindPropertyRelative("scaleX"), "Scale X", false, isAppearance, Color.red);
                scaleY = new FloatCurveDrawer(parent.FindPropertyRelative("scaleY"), "Scale Y", false, isAppearance, Color.green);

                rotX = new FloatCurveDrawer(parent.FindPropertyRelative("rotX"), "Rotation X", false, isAppearance, Color.red);
                rotY = new FloatCurveDrawer(parent.FindPropertyRelative("rotY"), "Rotation Y", false, isAppearance, Color.green);
                rotZ = new FloatCurveDrawer(parent.FindPropertyRelative("rotZ"), "Rotation Z", false, isAppearance, Color.cyan);

                color = new ColorCurveDrawer(parent.FindPropertyRelative("color"), "Color");
            }

            public void ResetValues()
            {
                effectTag.stringValue = string.Empty;

                int appearanceOffset = isAppearance ? 3 : 0;

                if (!isAppearance)
                {
                    emission.ResetValues();
                }

                movementY.ResetValues(0 + appearanceOffset);
                movementZ.ResetValues(0 + appearanceOffset);
                movementX.ResetValues(0 + appearanceOffset);

                scaleX.ResetValues(1 + appearanceOffset);
                scaleY.ResetValues(1 + appearanceOffset);

                rotX.ResetValues(2 + appearanceOffset);
                rotY.ResetValues(2 + appearanceOffset);
                rotZ.ResetValues(2 + appearanceOffset);

                color.ResetValues(isAppearance);
            }

            public void Show()
            {
                bool notLongEnough = !TextAnimator.IsTagLongEnough(effectTag.stringValue);
                //tag is short
                if (notLongEnough)
                {
                    GUI.backgroundColor = errorColor;
                }

                EditorGUI.BeginChangeCheck();
                if (Application.isPlaying)
                {
                    GUI.enabled = false;
                }
                EditorGUILayout.PropertyField(effectTag);
                if (notLongEnough)
                {
                    EditorGUILayout.LabelField("[!] This tag is too short.", EditorStyles.miniLabel);
                }

                if (Application.isPlaying)
                {
                    EditorGUILayout.LabelField("(You can't edit the tag IDs while in playmode.)", EditorStyles.centeredGreyMiniLabel);
                    GUI.enabled = true;
                }

                GUI.backgroundColor = Color.white;

                if (EditorGUI.EndChangeCheck())
                {
                    effectTag.stringValue = effectTag.stringValue.Replace(" ", "");
                }

                if (!isAppearance)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("--Emission--", EditorStyles.centeredGreyMiniLabel);
                    emission.Show();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Movement--", EditorStyles.centeredGreyMiniLabel);
                movementX.Show();
                movementY.Show();
                movementZ.Show();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Scale--", EditorStyles.centeredGreyMiniLabel);
                scaleX.Show();
                scaleY.Show();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Rotation--", EditorStyles.centeredGreyMiniLabel);
                rotX.Show();
                rotY.Show();
                rotZ.Show();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Color--", EditorStyles.centeredGreyMiniLabel);
                color.Show();
                EditorGUILayout.EndVertical();

            }
        }

        class FloatCurveDrawer
        {
            string name;
            bool sizeDependant;
            SerializedProperty enabled;
            SerializedProperty amplitude;
            SerializedProperty curve;
            SerializedProperty charsTimeOffset;
            GUIContent curveLabel;
            Color curveColor;

            string curveDescription;

            public FloatCurveDrawer(SerializedProperty parent, string name, bool sizeDependant, bool isAppearance, Color curveColor)
            {
                curveLabel = new GUIContent(isAppearance ? "Decay" : "Curve");
                this.sizeDependant = sizeDependant;
                this.name = (sizeDependant ? "[!] " : "") + name;
                this.curveColor = curveColor;

                amplitude = parent.FindPropertyRelative("amplitude");
                curve = parent.FindPropertyRelative("curve");
                enabled = parent.FindPropertyRelative("enabled");
                charsTimeOffset = parent.FindPropertyRelative("charsTimeOffset");
                CalculateCurveStats();
            }

            public void ResetValues(int type)
            {
                bool isAppearance = type >= 3;

                switch (type)
                {
                    //mov
                    default:
                    case 0:

                        amplitude.floatValue = 1;

                        curve.animationCurveValue = new AnimationCurve(
                                new Keyframe(0, 0, -5.4f, -5.4f),
                                new Keyframe(.25f, -1, -1, -1),
                                new Keyframe(.75f, 1, -1, -1),
                                new Keyframe(1, 0, -5.4f, -5.4f)
                            );
                        break;

                    //scale
                    case 1:
                        amplitude.floatValue = 2;

                        curve.animationCurveValue = new AnimationCurve(
                            new Keyframe(0, .5f),
                            new Keyframe(.5f, 1f),
                            new Keyframe(1, .5f)
                        );

                        break;

                    //rot
                    case 2:
                        amplitude.floatValue = 35; //angle of 35

                        curve.animationCurveValue = new AnimationCurve(
                            new Keyframe(0, 0, -5.4f, -5.4f),
                            new Keyframe(.25f, -1, -1, -1),
                            new Keyframe(.75f, 1, -1, -1),
                            new Keyframe(1, 0, -5.4f, -5.4f)
                        );
                        break;

                    //app mov
                    case 3:

                        amplitude.floatValue = 1;

                        curve.animationCurveValue = new AnimationCurve(
                            new Keyframe(0, 0),
                            new Keyframe(1, 1)
                        );

                        break;

                    //app scale
                    case 4:
                        amplitude.floatValue = 2;

                        curve.animationCurveValue = new AnimationCurve(
                            new Keyframe(0, 0),
                            new Keyframe(1, 1)
                        );

                        break;

                    //app rot
                    case 5:
                        amplitude.floatValue = 35; //angle of 35

                        curve.animationCurveValue = new AnimationCurve(
                            new Keyframe(0, 0),
                            new Keyframe(1, 1)
                        );
                        break;

                }

                //duration = isAppearance ? .3f : 1;
                curve.animationCurveValue.postWrapMode = isAppearance ? WrapMode.Once : WrapMode.Loop; //loops if behavior
                charsTimeOffset.floatValue = 0;

            }

            void CalculateCurveStats()
            {
                curveDescription = $"Duration: {curve.animationCurveValue.GetDuration()}\n{curve.animationCurveValue.preWrapMode} - {curve.animationCurveValue.postWrapMode}";

            }

            public void Show()
            {
                enabled.boolValue = EditorGUILayout.ToggleLeft(name, enabled.boolValue);

                if (enabled.boolValue)
                {
                    if (sizeDependant)
                    {
                        EditorGUILayout.LabelField(alertTextSizeDep, EditorStyles.centeredGreyMiniLabel);
                    }

                    EditorStyles.wordWrappedMiniLabel.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.LabelField(curveDescription, EditorStyles.wordWrappedMiniLabel);
                    EditorStyles.wordWrappedMiniLabel.alignment = TextAnchor.UpperLeft;

                    CalculateCurveStats(); //alyways calculating, because user may Undo and the duration could change


                    curve.animationCurveValue = EditorGUILayout.CurveField(curveLabel, curve.animationCurveValue, curveColor, Rect.zero);
                    //EditorGUILayout.PropertyField(curve, curveLabel);

                    EditorGUILayout.PropertyField(amplitude);
                    EditorGUILayout.PropertyField(charsTimeOffset);
                    GUILayout.Space(4);
                    //EditorGUILayout.Space();
                }
            }
        }
        [System.Serializable]
        class ColorCurveDrawer
        {

            string name;
            SerializedProperty enabled;
            SerializedProperty gradient;
            SerializedProperty duration;
            SerializedProperty charsTimeOffset;

            public ColorCurveDrawer(SerializedProperty parent, string name)
            {
                this.name = name;
                gradient = parent.FindPropertyRelative("gradient");
                enabled = parent.FindPropertyRelative("enabled");
                duration = parent.FindPropertyRelative("duration");
                charsTimeOffset = parent.FindPropertyRelative("charsTimeOffset");
            }
            public void ResetValues(bool isAppearance)
            {
                //TODO Find a way to show serialized property gradients

                if (isAppearance)
                {
                    /*
                    gradient = new GradientColorKey[] {
                        new GradientColorKey(Color.cyan, 0),
                        new GradientColorKey(Color.black, 1)
                    };

                    gradient.alphaKeys = new GradientAlphaKey[] {
                        new GradientAlphaKey(0, 0),
                        new GradientAlphaKey(1, 1)
                    };
                    */

                    duration.floatValue = 1;
                }
                else
                {/*
                    gradient.colorKeys = new GradientColorKey[] {
                    new GradientColorKey(Color.black, 0),
                    new GradientColorKey(Color.red, .5f),
                    new GradientColorKey(Color.black, 1)
                };

                    gradient.alphaKeys = new GradientAlphaKey[] {
                    new GradientAlphaKey(1, 0),
                    new GradientAlphaKey(1, 1)
                };
                */
                    duration.floatValue = 1;
                }

                charsTimeOffset.floatValue = 0;
            }

            public void Show()
            {
                enabled.boolValue = EditorGUILayout.ToggleLeft(name, enabled.boolValue);

                if (enabled.boolValue)
                {
                    EditorGUILayout.PropertyField(gradient);
                    EditorGUILayout.PropertyField(duration);
                    EditorGUILayout.PropertyField(charsTimeOffset);
                    EditorGUILayout.Space();
                }
            }

        }

        class EmissionCurveDrawer
        {
            string infoText;

            SerializedProperty cycles;
            SerializedProperty overrideDuration;
            SerializedProperty attackCurve;
            SerializedProperty decayCurve;
            SerializedProperty continueForever;

            public EmissionCurveDrawer(SerializedProperty parent)
            {
                this.infoText = string.Empty;
                attackCurve = parent.FindPropertyRelative("attackCurve");
                decayCurve = parent.FindPropertyRelative("decayCurve");
                cycles = parent.FindPropertyRelative("cycles");
                overrideDuration = parent.FindPropertyRelative("overrideDuration");
                continueForever = parent.FindPropertyRelative("continueForever");
            }

            public void ResetValues()
            {
                attackCurve.animationCurveValue = new AnimationCurve(
                    new Keyframe(0, 0),
                    new Keyframe(1, 1));

                continueForever.boolValue = true;

                decayCurve.animationCurveValue = new AnimationCurve(
                    new Keyframe(0, 1),
                    new Keyframe(1, 0));
            }

            public void Show()
            {
                infoText = $"Repeats { ((continueForever.boolValue || cycles.intValue <= 0) ? "forever" : (cycles.intValue + " time(s)"))}";

                GUI.enabled = false;
                EditorGUILayout.LabelField(infoText, EditorStyles.wordWrappedMiniLabel);
                EditorGUILayout.LabelField("Pro tip: Set the 'attack curve' keys to 1 to start the effect immediately", EditorStyles.wordWrappedMiniLabel);
                GUI.enabled = true;

                EditorGUILayout.CurveField(attackCurve, Color.yellow, Rect.zero);

                EditorGUILayout.PropertyField(continueForever);
                if (!continueForever.boolValue)
                {
                    EditorGUILayout.PropertyField(overrideDuration);
                    EditorGUILayout.CurveField(decayCurve, Color.yellow, Rect.zero);
                    EditorGUILayout.PropertyField(cycles);
                }

            }
        }

        #endregion

        #region Variables

        GUIContent easyIntegrationLabel = new GUIContent("Use Easy Integration");
        SerializedProperty triggerTypeWriter;
        SerializedProperty timeScale;

        SerializedProperty behaviorValues;
        SerializedProperty behavDef;
        SerializedProperty behCustomValues;
        SerializedProperty behLocalPresetsArray;

        SerializedProperty appLocalPresetsArray;


        SerializedProperty effectIntensity;
        SerializedProperty useDynamicScaling;
        SerializedProperty referenceFontSize;

        SerializedProperty appearanceValues;

        UserPresetDrawer[] behPresets = new UserPresetDrawer[0];
        UserPresetDrawer[] appPresets = new UserPresetDrawer[0];

        AppearanceDefaultEffects appDefaultPreset;
        BehaviorDefaultEffects behDefaultDrawer;

        bool behShowPresets;
        bool appShowPresets;

        #endregion

        public enum Show
        {
            Behaviors,
            Appearances
        }

        Show currentPanel;

        void MatchPresetsDrawersWithComponent()
        {
            MatchPresetsDrawersWithComponent(ref behPresets, behLocalPresetsArray, false);
            MatchPresetsDrawersWithComponent(ref appPresets, appLocalPresetsArray, true);
        }

        static void MatchPresetsDrawersWithComponent(ref UserPresetDrawer[] drawers, SerializedProperty arrayProperty, bool isAppearance)
        {
            if (drawers.Length != arrayProperty.arraySize)
            {
                drawers = new UserPresetDrawer[arrayProperty.arraySize];

                for (int i = 0; i < arrayProperty.arraySize; i++)
                {
                    drawers[i] = new UserPresetDrawer(arrayProperty.GetArrayElementAtIndex(i), isAppearance);
                }
            }
        }

        private void OnEnable()
        {
            effectIntensity = serializedObject.FindProperty("effectIntensityMultiplier");
            referenceFontSize = serializedObject.FindProperty("referenceFontSize");
            useDynamicScaling = serializedObject.FindProperty("useDynamicScaling");

            triggerTypeWriter = serializedObject.FindProperty("triggerAnimPlayerOnChange");
            timeScale = serializedObject.FindProperty("timeScale");

            availableAppBuiltinTagsLongText = string.Empty;
            for (int i = 0; i < TAnimTags.defaultAppearances.Length; i++)
            {
                availableAppBuiltinTagsLongText += TAnimTags.defaultAppearances[i] + ", ";
            }

            behaviorValues = serializedObject.FindProperty("behaviorValues");
            behCustomValues = behaviorValues.FindPropertyRelative("customs");

            #region Default Behaviors
            behavDef = behaviorValues.FindPropertyRelative("defaults");
            behDefaultDrawer = new BehaviorDefaultEffects(behavDef);


            #endregion

            #region Default Appearances
            appearanceValues = serializedObject.FindProperty("appearancesContainer");
            appDefaultPreset = new AppearanceDefaultEffects(appearanceValues);

            #endregion


            behLocalPresetsArray = behaviorValues.FindPropertyRelative("presets");

            appLocalPresetsArray = appearanceValues.FindPropertyRelative("values").FindPropertyRelative("presets");
            MatchPresetsDrawersWithComponent();

        }

        #region Styles Methods
        public static void WhatToShowButton(ref Show currentPanel)
        {
            void ShowButtonFor(ref Show panel, Show target)
            {
                string name = target == Show.Appearances ? "Edit Appearances" : "Edit Behaviors";
                if (panel == target)
                    GUI.backgroundColor = selectedShowColor;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(30));

                if (GUILayout.Button(name, EditorStyles.boldLabel))
                {
                    panel = target;
                }

                EditorGUILayout.EndVertical();

                GUI.backgroundColor = Color.white;
            }

            EditorGUILayout.BeginHorizontal();
            ShowButtonFor(ref currentPanel, Show.Behaviors);
            ShowButtonFor(ref currentPanel, Show.Appearances);
            EditorGUILayout.EndHorizontal();
        }

        void WhatToShowTitle(string content)
        {
            EditorGUILayout.LabelField(content, EditorStyles.centeredGreyMiniLabel);
        }
        #endregion

        static void ShowCustomEffectsValues(SerializedProperty property)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.PropertyField(property, true);

            EditorGUILayout.EndVertical();
        }

        internal static void ShowPresets(ref UserPresetDrawer[] userPresets, ref bool canShow, ref SerializedProperty arrayProperty, bool isAppearance)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.BeginChangeCheck();
            canShow = EditorGUILayout.BeginToggleGroup($"Edit presets effects [{arrayProperty.arraySize} created]", canShow);

            #region CanShow variable changed
            if (EditorGUI.EndChangeCheck())
            {
                //Resets "confirmation to delete effect" button
                for (int i = 0; i < userPresets.Length; i++)
                {
                    userPresets[i].wantsToRemove = false;
                }
            }
            #endregion

            #region Shows Preset
            if (canShow)
            {
                //Checks for error
                MatchPresetsDrawersWithComponent(ref userPresets, arrayProperty, isAppearance);

                for (int i = 0; i < userPresets.Length; i++)
                {

                    #region Header
                    if (userPresets[i].show)
                        GUI.backgroundColor = expandedColor;
                    else
                        GUI.backgroundColor = notExpandedColor;

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    GUI.backgroundColor = Color.white;

                    if (userPresets[i].show)
                    {
                        EditorGUILayout.BeginHorizontal();
                    }

                    if (GUILayout.Button($"-> {userPresets[i].getName}", userPresets[i].show ? EditorStyles.boldLabel : EditorStyles.label))
                    {
                        userPresets[i].show = !userPresets[i].show;
                        userPresets[i].wantsToRemove = false;
                    }
                    #endregion

                    #region Body

                    if (userPresets[i].show)
                    {
                        if (Application.isPlaying)
                            GUI.enabled = false;

                        if (GUILayout.Button(userPresets[i].wantsToRemove ? "Confirm?" : "Remove?", EditorStyles.miniButtonRight, GUILayout.Width(85)))
                        {
                            //Confirms remove
                            if (userPresets[i].wantsToRemove)
                            {
                                arrayProperty.DeleteArrayElementAtIndex(i);
                                MatchPresetsDrawersWithComponent(ref userPresets, arrayProperty, isAppearance);
                                break;
                            }
                            else //asks for remove
                            {
                                userPresets[i].wantsToRemove = true;
                            }
                        }

                        GUI.enabled = true;

                        EditorGUILayout.EndHorizontal();

                        GUI.backgroundColor = Color.white;

                        //can add modules only if it's a local effect or player is in edit mode
                        userPresets[i].Show();
                    }

                    #endregion

                    EditorGUILayout.EndToggleGroup();
                }

                #region Add New
                if (Application.isPlaying) //prevents adding new preset if in play mode
                    GUI.enabled = false;

                if (GUILayout.Button("Add new", EditorStyles.miniButtonMid) && !Application.isPlaying)
                {
                    arrayProperty.InsertArrayElementAtIndex(Mathf.Clamp(arrayProperty.arraySize - 1, 0, arrayProperty.arraySize));
                    MatchPresetsDrawersWithComponent(ref userPresets, arrayProperty, isAppearance);
                    userPresets[userPresets.Length - 1].ResetValues();
                }

                GUI.enabled = true;
                #endregion
            }

            #endregion

            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.EndVertical();

        }


        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(timeScale);

            EditorGUILayout.LabelField("With the below value, amplify the effects that are dependant on different sizes", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.PropertyField(effectIntensity);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(useDynamicScaling);
            if (EditorGUI.EndChangeCheck())
            {
                if (useDynamicScaling.boolValue)
                {
                    if (referenceFontSize.floatValue <= 0)
                    {
                        var tmproText = (target as TextAnimator)?.GetComponent<TMPro.TMP_Text>();
                        if (tmproText.text != null)
                        {
                            referenceFontSize.floatValue = tmproText.fontSize;
                        }
                    }
                }
            }

            if (useDynamicScaling.boolValue)
                EditorGUILayout.PropertyField(referenceFontSize);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(triggerTypeWriter, easyIntegrationLabel);

            if (triggerTypeWriter.boolValue)
            {
                EditorGUILayout.LabelField("- Be sure to add a TextAnimatorPlayer component.\n", EditorStyles.wordWrappedMiniLabel);
            }
            else
            {
                EditorGUILayout.LabelField("[!!] If you're changing text directly from TMPro frequently (typewriter-like), please set this to true or performance may vary. Read docs for more <3", EditorStyles.wordWrappedMiniLabel);
            }

            EditorGUILayout.Space();

            //Chooses what to show
            WhatToShowButton(ref currentPanel);

            //Shows based on the type
            switch (currentPanel)
            {
                case Show.Behaviors:

                    WhatToShowTitle("Currently showing Behavior Effects");
                    behDefaultDrawer.Show();

                    ShowPresets(ref behPresets, ref behShowPresets, ref behLocalPresetsArray, false);

                    ShowCustomEffectsValues(behCustomValues);

                    break;

                case Show.Appearances:

                    WhatToShowTitle("Currently showing Appearance Effects");
                    appDefaultPreset.Show();

                    ShowPresets(ref appPresets, ref appShowPresets, ref appLocalPresetsArray, true);

                    appDefaultPreset.ShowCustom();

                    break;
            }

            void ResetEffects()
            {
                ((TextAnimator)target)?.SendMessage("EDITORONLY_ResetEffects", SendMessageOptions.RequireReceiver); //Resets effects on the target script
            }

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();

                if (Application.isPlaying)
                    ResetEffects();
            }

            //Editor Helpers
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Editor Helper Buttons ->", GUILayout.MaxWidth(150));

                if (GUILayout.Button("Locate Global Data", EditorStyles.miniButton))
                {
                    TAnimGlobalDataScriptableDrawer.LocateGlobalData();
                }
                GUI.enabled = Application.isPlaying;
                if (GUILayout.Button("Reset Behaviors", EditorStyles.miniButton))
                {
                    ResetEffects();
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }
    }

}