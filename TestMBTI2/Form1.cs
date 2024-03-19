using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace TestMBTI2
{
    public partial class Form1 : Form
    {
        DateTime date = DateTime.Now;
        private int userId;
        private int idresult;
        private int idTest;
        private Dbdriver dbDriver = new Dbdriver();
        private SqlDataReader reader;
        private int currentQuestionIndex1 = 0;
        private int currentQuestionIndex2 = 0;
        private List<int> answers = new List<int>();
        private int totalQuestions;
        private Dictionary<string, int> scaleResults = new Dictionary<string, int>
        {
            { "E", 0 },
            { "I", 0 },
            { "S", 0 },
            { "N", 0 },
            { "T", 0 },
            { "F", 0 },
            { "J", 0 },
            { "P", 0 }
        };
        private Dictionary<string, string> oppositeScalesDictionary = new Dictionary<string, string>
{
    { "E", "I" },
    { "I", "E" },
    { "S", "N" },
    { "N", "S" },
    { "T", "F" },
    { "F", "T" },
    { "J", "P" },
    { "P", "J" }
};
        private Dictionary<string, string> descriptionsDictionary = new Dictionary<string, string>
{
    { "ISTJ", "Интроверт. Сенсорный. Мыслитель. Судья." },
    { "ISFJ", "Интроверт. Сенсорный. Чувствующий. Судья." },
    { "INFJ", "Интроверт. Интуитивный. Чувствующий. Судья." },
    { "INTJ", "Интроверт. Интуитивный. Мыслитель. Судья." },
    { "ISTP", "Интроверт. Сенсорный. Мыслитель. Перцептуалист." },
    { "ISFP", "Интроверт. Сенсорный. Чувствующий. Перцептуалист." },
    { "INFP", "Интроверт. Интуитивный. Чувствующий. Перцептуалист." },
    { "INTP", "Интроверт. Интуитивный. Мыслитель. Перцептуалист." },
    { "ESTP", "Экстраверт. Сенсорный. Мыслитель. Перцептуалист." },
    { "ESFP", "Экстраверт. Сенсорный. Чувствующий. Перцептуалист." },
    { "ENFP", "Экстраверт. Интуитивный. Чувствующий. Перцептуалист." },
    { "ENTP", "Экстраверт. Интуитивный. Мыслитель. Перцептуалист." },
    { "ESTJ", "Экстраверт. Сенсорный. Мыслитель. Судья." },
    { "ESFJ", "Экстраверт. Сенсорный. Чувствующий. Судья." },
    { "ENFJ", "Экстраверт. Интуитивный. Чувствующий. Судья." },
    { "ENTJ", "Экстраверт. Интуитивный. Мыслитель. Судья." }
};
        private Dictionary<string, string> recommendationsDictionary = new Dictionary<string, string>
{
    { "ISTJ", "Рекомендуется заниматься профессиями, связанными с анализом данных, бухгалтерией, администрированием, юриспруденцией." },
    { "ISFJ", "Рекомендуется заниматься профессиями, связанными с образованием, здравоохранением, социальной работой." },
    { "INFJ", "Рекомендуется заниматься профессиями, связанными с искусством, музыкой, литературой, психологией." },
    { "INTJ", "Рекомендуется заниматься профессиями, связанными с наукой, исследованиями, разработкой." },
    { "ISTP", "Рекомендуется заниматься профессиями, связанными с техникой, ремонтом, строительством." },
    { "ISFP", "Рекомендуется заниматься профессиями, связанными с дизайном, модой, фотографией." },
    { "INFP", "Рекомендуется заниматься профессиями, связанными с писательством, журналистикой, музыкой." },
    { "INTP", "Рекомендуется заниматься профессиями, связанными с наукой, исследованиями, разработкой." },
    { "ESTP", "Рекомендуется заниматься профессиями, связанными с продажами, маркетингом, туризмом." },
    { "ESFP", "Рекомендуется заниматься профессиями, связанными с обслуживанием, развлечениями, туризмом." },
    { "ENFP", "Рекомендуется заниматься профессиями, связанными с писательством, журналистикой, музыкой." },
    { "ENTP", "Рекомендуется заниматься профессиями, связанными с наукой, исследованиями, разработкой." },
    { "ESTJ", "Рекомендуется заниматься профессиями, связанными с управлением, администрированием, бизнесом." },
    { "ESFJ", "Рекомендуется заниматься профессиями, связанными с образованием, здравоохранением, социальной работой." },
    { "ENFJ", "Рекомендуется заниматься профессиями, связанными с искусством, музыкой, литературой, психологией." },
    { "ENTJ", "Рекомендуется заниматься профессиями, связанными с наукой, исследованиями, разработкой." }
};
        public Form1(int userId)
        {
            InitializeComponent();
            LoadQuestion();
            StartPosition = FormStartPosition.CenterScreen;
            this.userId = userId;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void LoadQuestion()
        {
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }
            string query = "SELECT idQuest, TextQuest FROM Вопросы WHERE idQuest >= @CurrentId ORDER BY idQuest ASC";
            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@CurrentId", currentQuestionIndex1);

                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    
                    int idQuest = Convert.ToInt32(reader["idQuest"]);
                    labelQuestion.Text = reader["TextQuest"].ToString();
                    currentQuestionIndex1 = idQuest;

                    LoadAnswers(idQuest);
                   
                }
                else
                {
                    MessageBox.Show("Вопросов больше нет.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке вопроса: " + ex.Message);
            }
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }
            dbDriver.openConnection();
            string query1 = "SELECT COUNT(*) FROM Вопросы";
            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query1, dbDriver.getConnection());

                totalQuestions = Convert.ToInt32(command.ExecuteScalar());

                labelQuestionCounter.Text = "Вопрос:"+(currentQuestionIndex2 + 1) + "/" + totalQuestions;
                currentQuestionIndex2++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении количества вопросов: " + ex.Message);
            }
        }
        private void LoadAnswers(int questionId)
        {
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }
           

            string query = "SELECT idAns, ansver, ansver2 FROM Ответы1 WHERE idAns = @AnsId";
            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@AnsId", questionId);

                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    radioButton1.Text = reader["ansver"].ToString();
                    radioButton1.Tag = reader.GetInt32(0);

                    radioButton2.Text = reader["ansver2"].ToString();
                    radioButton2.Tag = reader.GetInt32(0);
                }
                else
                {
                    MessageBox.Show("Ответов не найдено.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке ответов: " + ex.Message);
            }
        }
        private void SaveResults()
        {
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }
            for (int i = 0; i < answers.Count; i++)
            {
                string scaleName = GetScaleName(i);

                if (answers[i] == 1)
                {
                    scaleResults[GetOppositeScaleName(scaleName)]++;
                }
                else if (answers[i] == 2)
                {
                    scaleResults[scaleName]++;
                }
            }
            string personalityType = GetPersonalityType(scaleResults);
            string description = GetDescription(personalityType);
            string recommendations = GetRecommendations(personalityType);

            string query = "INSERT INTO Результаты (TypeMBTI, descriptionType, recomendation) OUTPUT inserted.idresult VALUES (@TypeMBTI, @descriptionType, @recomendation)";

            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@TypeMBTI", personalityType);
                command.Parameters.AddWithValue("@descriptionType", description);
                command.Parameters.AddWithValue("@recomendation", recommendations);

                int idresult = (int)command.ExecuteScalar();

                string query7 = "INSERT INTO Тест (Пользователь_iduser, Date) OUTPUT inserted.idTest VALUES (@IdUser, @Date)";

                    dbDriver.openConnection();
                    SqlCommand command7 = new SqlCommand(query7, dbDriver.getConnection());
                    command7.Parameters.AddWithValue("@IdUser", userId);
                    command7.Parameters.AddWithValue("@Date", date);

                    int idTest = (int)command7.ExecuteScalar();
                    MessageBox.Show("Результаты теста сохранены.");
                ResultsForm resultsForm = new ResultsForm(personalityType, description, recommendations, userId, idTest, idresult);
                resultsForm.Show();
                
                
                this.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении результатов теста: " + ex.Message);
            }
            finally
            {
                dbDriver.closeConnection();
                this.Close();
            }
        }
        private string GetScaleName(int index)
        {
            string[] scaleNames = { "E", "E", "S", "S", "T", "T", "J", "J" };
            return scaleNames[index % 8];
        }

        private string GetOppositeScaleName(string scaleName)
        {
            string oppositeScaleName = "";

            if (oppositeScalesDictionary.TryGetValue(scaleName, out oppositeScaleName))
            {
                return oppositeScaleName;
            }
            else
            {
                return "Нет противоположной шкалы для этого типа личности.";
            }
        }
        private string GetPersonalityType(Dictionary<string, int> scaleResults)
        {
            string personalityType = "";

            personalityType += scaleResults["E"] > scaleResults["I"] ? "E" : "I";
            personalityType += scaleResults["S"] > scaleResults["N"] ? "S" : "N";
            personalityType += scaleResults["T"] > scaleResults["F"] ? "T" : "F";
            personalityType += scaleResults["J"] > scaleResults["P"] ? "J" : "P";

            return personalityType;
        }

        private string GetDescription(string personalityType)
        {
            string description = "";

            if (descriptionsDictionary.TryGetValue(personalityType, out description))
            {
                return description;
            }
            else
            {
                return "Нет описания для этого типа личности.";
            }
        }

        private string GetRecommendations(string personalityType)
        {
            string recommendations = "";

            if (recommendationsDictionary.TryGetValue(personalityType, out recommendations))
            {
                return recommendations;
            }
            else
            {
                return "Нет рекомендаций для этого типа личности.";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }

            if (radioButton1.Checked)
            {
                answers.Add(1);
                radioButton1.Checked = true;
                
            }
            else if (radioButton2.Checked)
            {
                answers.Add(2);
                radioButton1.Checked = true;
            }

            currentQuestionIndex1++;

            if (currentQuestionIndex1 <= GetLastQuestionId())
            {
                LoadQuestion();
            }
            else
            {
                SaveResults();
            }
        }

        private int GetLastQuestionId()
        {
            string query = "SELECT MAX(idQuest) FROM Вопросы";
            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении последнего вопроса: " + ex.Message);
                return 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentQuestionIndex2 > 1)
            {
                currentQuestionIndex1--;
                currentQuestionIndex2 -= 2;
                LoadQuestion();
            }

        }
        

        
    }
}
