namespace GravyFoodsApi.Other
{
    public class IOClass
    {

        public static void WriteToTextFile(String strW, String strFileName = "log_" + ".txt")
        {

            //object value = File.WriteAllLinesAsync(strFileName, strW);

            //object str = File.WriteAllLines(strFileName,1);

            //String[] stringArr = new String[1];
            //stringArr[0] = strW;

            //String[] stringArr = new String[1] { strW };

            //FileStream uploadFileStream = File.WriteAllText(strFileName,strW);


            TextWriter tw = new StreamWriter("C:/Response.txt");

            // write a line of text to the file
            tw.WriteLine(strW);

            // close the stream
            tw.Close();


        }
    }
}
