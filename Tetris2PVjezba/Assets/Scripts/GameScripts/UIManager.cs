using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScene
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _player1Score;
        [SerializeField] private TextMeshProUGUI _player2Score;
        [SerializeField] private TextMeshProUGUI _player1LinesCleared;
        [SerializeField] private TextMeshProUGUI _player2LinesCleared;
        [SerializeField] private TextMeshProUGUI _player1PPS;
        [SerializeField] private TextMeshProUGUI _player2PPS;
        [SerializeField] private GameObject _ready;
        [SerializeField] private GameObject _go;
        [SerializeField] private GameObject _playerWins;
        [SerializeField] private Button _playAgain;
        [SerializeField] private Button _quit;
        [SerializeField] private string _player1Name = null;
        [SerializeField] private string _player2Name = null;
        [SerializeField] private  LoadData _loadData = null;
        private int _sumLinesCleared1;
        private int _sumLinesCleared2;
        private IEnumerator _countPPS;
        private float _startTime;

        public int PiecesPlaced1;
        public int PiecesPlaced2;

        private static UIManager _instance;
        public static UIManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            Init();
        }

        private void Init()
        {
            _playAgain.onClick.AddListener(Restart);
            _quit.onClick.AddListener(QuitGame);
        }

        void OnEnable()
        {
            TetrominoController.GameEnd += GameEndScreen;
        }
        void OnDisable()
        {
            TetrominoController.GameEnd -= GameEndScreen;
        }

        private void Start()
        {
            SetScores();
            StartCoroutine(StartingGame());
        }

        private IEnumerator PPSCounter()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                _player1PPS.SetText("PIECES PER SECOND: " + String.Format("{0:0.00}", PiecesPlaced1 / (Time.time - _startTime)));
                _player2PPS.SetText("PIECES PER SECOND: " + String.Format("{0:0.00}", PiecesPlaced2 / (Time.time - _startTime)));
            }
        }

        private IEnumerator StartingGame()
        {
            //Aktivirajte korituinu za znak Ready
            yield return new WaitForSeconds(1);
            //deaktivirajte korutinu za znak Ready
            _ready.SetActive(false);
            //aktivirajte korituinu za znak Go!
             yield return new WaitForSeconds(1);
            //deaktivirajte korutinu za znak Go!
            _go.SetActive(false);


            _countPPS = PPSCounter();
             StartCoroutine(_countPPS);  
            //pokrenuti korutinu za broj spuštenih tetronima

      //pohraniti vrijeme početka
            _startTime = Time.time;

            //pozvati metodu GameStart() instance klase GameManager
            GameManager gameManagerInstance = GameManager.Instance;
            gameManagerInstance.GameStart();

    }

    private void SetScores()
        {
            _player1Score.SetText(_loadData.score1.ToString());
            _player2Score.SetText(_loadData.score2.ToString());
        }

        private void GameEndScreen(string playerName)
        {
            StopCoroutine(_countPPS);
            _playerWins.SetActive(true);

            if (string.Equals(_player2Name, playerName))
            {
                _loadData.score1++;
                _player1Score.SetText(_loadData.score1.ToString());
                _playerWins.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(_player1Name + " WINS!");
            }
            else
            {
                _loadData.score2++;
                _player2Score.SetText(_loadData.score2.ToString());
                _playerWins.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(_player2Name + " WINS!");
            }

            _playAgain.gameObject.SetActive(true);
            _quit.gameObject.SetActive(true);
        }

        private void Restart()
        {
            SceneManager.LoadScene("TetrisMainScene");
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        public void SumLinesClearedPlayer1(int lines)
        {
          /*
         * Todo: 
         * Implementirati ažuriranje i prikaz broja
         * poèišæenih linija igraèa 1.
         */
          _sumLinesCleared1 += lines;
          _player1LinesCleared.SetText("NUMBER OF CLEARED LINES: " + String.Format("{0}", _sumLinesCleared1));

    }

    public void SumLinesClearedPlayer2(int lines)
        {
          /*
         * Todo: 
         * Implementirati ažuriranje i prikaz broja
         * poèišæenih linija igraèa 2.  
         */
          _sumLinesCleared2 += lines;

          _player2LinesCleared.SetText("NUMBER OF CLEARED LINES: " + String.Format("{0:0.00}", _sumLinesCleared2));

    }
  }
}
