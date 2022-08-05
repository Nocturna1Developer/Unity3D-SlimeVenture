using UnityEngine;
using UnityEditor;

namespace CodeMonkey.HealthSystemCM {

    [CustomEditor(typeof(Readme_HealthSystem))]
    [InitializeOnLoad]
    public class ReadmeEditor_HealthSystem : Editor {

        private static float kSpace = 16f;

        protected override void OnHeaderGUI() {
            Readme_HealthSystem readme = (Readme_HealthSystem)target;
            Init();

            GUILayout.BeginHorizontal("In BigTitle");

            float headerAspectRatio = (float)readme.codeMonkeyHeader.height / readme.codeMonkeyHeader.width;
            float width = EditorGUIUtility.currentViewWidth - 30f;
            GUILayout.Label(readme.codeMonkeyHeader, GUILayout.Width(width), GUILayout.Height(width * headerAspectRatio));

            GUILayout.EndHorizontal();

            GUILayout.Label(readme.title, HeaderStyle);
        }

        public override void OnInspectorGUI() {
            Readme_HealthSystem readme = (Readme_HealthSystem)target;
            Init();

            foreach (Readme_HealthSystem.Section section in readme.sections) {
                if (!string.IsNullOrEmpty(section.heading)) {
                    GUILayout.Label(section.heading, HeadingStyle);
                }
                if (section.textLines != null) {
                    foreach (string textLine in section.textLines) {
                        if (!string.IsNullOrEmpty(textLine)) {
                            string sectionText = textLine;
                            sectionText = sectionText.Replace("\\n", "\n");
                            GUILayout.Label(sectionText, BodyStyleSmall);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(section.linkText)) {
                    GUILayout.Space(kSpace / 2);
                    if (LinkLabel(new GUIContent(section.linkText))) {
                        Application.OpenURL(section.url);
                    }
                }
                GUILayout.Space(kSpace);
            }
        }


        private bool m_Initialized;

        private GUIStyle LinkStyle { get { return m_LinkStyle; } }
        [SerializeField] private GUIStyle m_LinkStyle;

        private GUIStyle HeaderStyle { get { return m_HeaderStyle; } }
        [SerializeField] private GUIStyle m_HeaderStyle;

        private GUIStyle TitleStyle { get { return m_TitleStyle; } }
        [SerializeField] private GUIStyle m_TitleStyle;

        private GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
        [SerializeField] private GUIStyle m_HeadingStyle;

        private GUIStyle BodyStyle { get { return m_BodyStyle; } }
        [SerializeField] private GUIStyle m_BodyStyle;

        private GUIStyle BodyStyleSmall { get { return m_BodyStyleSmall; } }
        [SerializeField] private GUIStyle m_BodyStyleSmall;

        private void Init() {
            if (m_Initialized)
                return;

            m_BodyStyle = new GUIStyle(EditorStyles.label);
            m_BodyStyle.wordWrap = true;
            m_BodyStyle.fontSize = 14;
            m_BodyStyle.richText = true;

            m_BodyStyleSmall = new GUIStyle(m_BodyStyle);
            m_BodyStyleSmall.fontSize = 12;

            m_TitleStyle = new GUIStyle(m_BodyStyle);
            m_TitleStyle.fontSize = 24;

            m_HeaderStyle = new GUIStyle(m_BodyStyle);
            m_HeaderStyle.fontSize = 24;
            m_HeaderStyle.fontStyle = FontStyle.Bold;
            m_HeaderStyle.alignment = TextAnchor.MiddleCenter;

            m_HeadingStyle = new GUIStyle(m_BodyStyle);
            m_HeadingStyle.fontSize = 18;
            m_HeadingStyle.fontStyle = FontStyle.Bold;

            m_LinkStyle = new GUIStyle(m_BodyStyle);
            // Match selection color which works nicely for both light and dark skins
            m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            m_LinkStyle.stretchWidth = false;

            m_Initialized = true;
        }

        private bool LinkLabel(GUIContent label, params GUILayoutOption[] options) {
            var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

            Handles.BeginGUI();
            Handles.color = LinkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, LinkStyle);
        }

    }

}