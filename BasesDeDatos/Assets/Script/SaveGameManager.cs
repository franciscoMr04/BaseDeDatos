using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class SaveGameManager : MonoBehaviour
{

    public Text textoPrueba;
    public Text guardar;

    //declaramos el string que queremos guardar
    public class Guardar
    {
        public string mensage;

    }
    public Guardar save;


    //aqui lo que hacemos es guardar el texto que muestra la carta, la encripta y guarda en nuestro ordenador 
    public void GuardarCarta()
    {

        Guardar objetoGuardar = new Guardar();

        objetoGuardar.mensage= "Hamburguesa\nPizza\nTortilla";


        string filePath = Application.persistentDataPath + "/archivoGuardado.sav";


        byte[] encryptedMessage = Encrypt(JsonConvert.SerializeObject(objetoGuardar));
        File.WriteAllBytes(filePath, encryptedMessage);

        Debug.Log(filePath);
        Debug.Log("Carta");


    }

    //despues de guardarla, lo que hacemos es que la cargue en el mismo sitio que la guarda
    public void CargarCarta()
    {

        string filePath = Application.persistentDataPath + "/archivoGuardado.sav";


        byte[] decryptedMessage = File.ReadAllBytes(filePath);
        string messagee = Decrypt(decryptedMessage);


        save = JsonConvert.DeserializeObject<Guardar>(messagee);

            Debug.Log(filePath);
        
    }

    
    //toda esta parte, creamos todo el sistema de encriptado y desencriptado del mensaje 
    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
    byte[] _inicializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    byte[] Encrypt(string message)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _inicializationVector);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        streamWriter.WriteLine(message);

        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    string Decrypt(byte[] message)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform decrypter = aes.CreateDecryptor(_key, _inicializationVector);

        MemoryStream memoryStream = new MemoryStream(message);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        StreamReader streamReader = new StreamReader(cryptoStream);

        string decryptedMessage = streamReader.ReadToEnd();

        memoryStream.Close();
        cryptoStream.Close();
        streamReader.Close();

        return decryptedMessage;
    }
}
