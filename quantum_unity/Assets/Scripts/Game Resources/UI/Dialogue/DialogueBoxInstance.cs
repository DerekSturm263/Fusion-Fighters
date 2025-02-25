using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameResources.UI.Dialogue
{
    public class DialogueBoxInstance : MonoBehaviour
    {
        private TMPro.TMP_Text _speakerText;
        private TMPro.TMP_Text _dialogueText;

        private Dialogue _dialogue;
        private DialoguePiece CurrentDialoguePiece => _dialogue.Dialogues[_dialogueIndex];

        private StringBuilder _currentText = new();
        private float _currentDialogueTime;
        private int _dialogueCharIndex;

        private int _dialogueIndex;

        [SerializeField] private Transform _buttonParent;
        [SerializeField] private GameObject _buttonTemplate;

        public void Setup(Dialogue dialogue)
        {
            _dialogue = dialogue;

            TMPro.TMP_Text[] tmps = GetComponentsInChildren<TMPro.TMP_Text>();

            _speakerText = tmps[0];
            _dialogueText = tmps[1];

            if (_dialogue.HideSpeaker)
                _speakerText.SetText("???");
            else
                RevealSpeaker();

            CurrentDialoguePiece.PlayVoiceLines(this);
            CurrentDialoguePiece.InvokeEvent();
        }

        private void Update()
        {
            _currentDialogueTime += Time.deltaTime * CurrentDialoguePiece.Speed;

            if (_currentText.ToString() != CurrentDialoguePiece.Text && _currentDialogueTime > 0.1f)
            {
                _currentText.Append(CurrentDialoguePiece.Text[_dialogueCharIndex]);
                ++_dialogueCharIndex;

                _currentDialogueTime = 0;

                if (_currentText.ToString() == CurrentDialoguePiece.Text)
                {
                    SetResponses();
                }
            }

            _dialogueText.SetText(_currentText);
        }

        public void FinishOrNextDialogue()
        {
            if (_currentText.ToString() != CurrentDialoguePiece.Text)
                FinishDialogue();
            else
                NextDialogue();
        }

        public void NextDialogue()
        {
            if (_dialogueIndex + 1 >= _dialogue.Dialogues.Count)
            {
                EndDialogue();
                return;
            }

            _currentDialogueTime = 0;
            _currentText.Clear();
            _dialogueCharIndex = 0;

            foreach (Transform buttonTransform in _buttonParent.GetComponentInChildren<Transform>())
            {
                Destroy(buttonTransform.gameObject);
            }

            ++_dialogueIndex;

            StopAllCoroutines();
            CurrentDialoguePiece.PlayVoiceLines(this);
            CurrentDialoguePiece.InvokeEvent();
        }

        public void SetDialogueIndex(int index)
        {
            _currentDialogueTime = 0;
            _currentText.Clear();
            _dialogueCharIndex = 0;

            if (index >= _dialogue.Dialogues.Count)
            {
                EndDialogue();
                return;
            }

            _dialogueIndex = index;
        }

        private void EndDialogue()
        {
            DialogueController.Instance.Despawn();
            Invoke(nameof(InvokeDialogueEvents), _dialogue.EventDelay);
        }

        private void InvokeDialogueEvents()
        {
            _dialogue.InvokeOnDialogueEnd();
        }

        public void FinishDialogue()
        {
            _currentDialogueTime = 0;
            _currentText.Clear();
            _currentText.Append(CurrentDialoguePiece.Text);
            _dialogueCharIndex = 0;

            SetResponses();
        }

        private void SetResponses()
        {
            List<Button> responseButtons = new();

            foreach (KeyValuePair<string, UnityEvent> response in CurrentDialoguePiece.Responses)
            {
                Button button = Instantiate(_buttonTemplate, _buttonParent).GetComponent<Button>();

                button.GetComponentInChildren<TMPro.TMP_Text>().SetText(response.Key);
                button.onClick.AddListener(() => response.Value.Invoke());

                responseButtons.Add(button);
            }

            if (responseButtons.Count > 0)
                EventSystem.current.SetSelectedGameObject(responseButtons[0].gameObject);
        }

        public void RevealSpeaker()
        {
            _speakerText.SetText(_dialogue.Speaker);
        }
    }
}
