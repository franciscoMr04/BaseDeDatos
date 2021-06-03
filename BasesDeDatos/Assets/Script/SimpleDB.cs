using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class SimpleDB : MonoBehaviour
{
    public InputField foodInput;
    public InputField mealInput;
    public Text foodList;

    private string dbName = "URI = file:FoodLog.db";

    //llamamos a las funciones para crear y mostrar la tabla.
    void Start()
    {
        CreateDB();

        DisplayFood();
    }

    //creamos la tabla. 
    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS fooditems (foodname VARCHAR(30), meal VARCHAR(20));";
                command.ExecuteNonQuery();
            }

            connection.Clone();
        }
    }

    //aqui añadimos la comida y el tipo de comida a la tabla.
    public void AddFood()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO fooditems(foodname, meal) VALUES ('" + foodInput.text + "', '" + mealInput.text + "');";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        DisplayFood();
    }

    //la comida que añadimos, la añadimos a la tabla y la ordena por el tipo de comida
    public void DisplayFood()
    {
        foodList.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM fooditems ORDER BY meal;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())

                        foodList.text += reader["foodname"] + "\t\t" + reader["meal"] + "\n";

                    reader.Close();
                }
            }
        }
    }
}

