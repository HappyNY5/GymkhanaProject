using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using TMPro;
using System.IO;

public class CanvasWorker : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text textKRW;
    [SerializeField] private TMP_Text textCarPrice;
    private string url_BBR;
    private string url_Encar = "http://www.encar.com/dc/dc_cardetailview.do?pageid=dc_carsearch&listAdvType=pic&carid=34502602&view_type=checked&wtClick_korList=015&advClickPosition=kor_pic_p1_g1";

    private string startPosKRW;
    private string endPosKRW;

    private string startPosEncarPrice = "function gotwit(value){";
    private string endPosEncarPrice = "var readStr = encodeURI(str);";

    private string startPosEncarVolume;
    private string endPosEncarVolume;

    private string startPosEncarDate;
    private string endPosEncarDate;


    static float curRateKRW_float = 0;
    static float curCarPrice_float = 0;

    void Start()
    {
        url_BBR = "https://bbr.ru/chastnym-litsam/obmen-valyuty/?from=RUB&to=KRW&fsum=1000";
        startPosKRW = "KRW':{'ID':'21','NAME':'','CURRENCY':{'FROM':'RUB','TO':'KRW'},'PRICE':{'FROM':";
        endPosKRW = ",'TO':'1.00000'},'RATE':'1000','MIN_SUM':'0.0000','MAX_SUM':'999999999.0000'}," ;

        UpdatePrice();
    }

    public void UpdateRate()
    {
        StartCoroutine(LoadRateKRW(url_BBR, Debug.Log, textKRW, startPosKRW, endPosKRW, curRateKRW_float)); 
    }

    public void UpdatePrice()
    {
        StartCoroutine(LoadRateKRW(url_Encar, Debug.Log, textCarPrice, startPosEncarPrice, endPosEncarPrice, curCarPrice_float));
    }

    IEnumerator LoadRateKRW(string url, Action<string> response, TMP_Text curText, string strStart, string strEnd, float name_float)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            response(www.error);
        }
        else {
            response(www.downloadHandler.text);
        }
        curText.text = curText.text.Substring(0, 12) + GetBetween(www.downloadHandler.text, strStart, strEnd, name_float);
        response(curText.text+"\n"+name_float);
        WriteInFile(www.downloadHandler.text);
    }

    public static string GetBetween(string strSource, string strStart, string strEnd, float name)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length + 1;
            End = strSource.IndexOf(strEnd, Start) - 1;
            name = float.Parse(strSource.Substring(Start, End - Start));
            return strSource.Substring(Start, End - Start);
        }

        return "0";
    }

    private void WriteInFile(string text)
    {
        string path = "Assets/test.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }
}
