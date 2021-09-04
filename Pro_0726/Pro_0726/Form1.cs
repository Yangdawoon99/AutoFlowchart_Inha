using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pro_0726
{
    public partial class btn_Transform : Form
    {
        List<string> code_reverse = new List<string>(); // 코드 한줄씩 담을 리스트
        List<string> reverse_list = new List<string>(); // 보내줄 리스트

        public btn_Transform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //lblLeft.Text = "Hello World!";
            string theText = textBox1.Text;
            theText = theText.Replace("\t", "tab "); // tab키 tab 으로 대체
            theText = theText.Replace("    ", "tab "); // 4칸공백 tab 으로 대체
            while (theText.Contains("\r\n\r\n"))
            {
                theText = theText.Replace("\r\n\r\n", "\r\n"); // 빈줄제거
            }
            string theText2 = Transform(theText);
            while (theText2.Contains("\r\n\r\n"))
            {
                theText2 = theText2.Replace("\r\n\r\n", "\r\n"); // 빈줄제거
            }
            textBox2.Text = theText2;
            theText2 = theText2.Replace("    ", ""); // 4칸공백 제거
            string theText3 = Structure_Re(theText2);
            textBox3.Text = theText3;

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        static string Transform(String Text)
        {
            string Code(String result)
            {
                string reverse = "";
                if (result == "tab")
                {
                    reverse = "    ";//들여쓰기
                }
                else if (result == "for")
                {
                    reverse = "시작-for문"; //평행사변형
                }
                else if (result == "while")
                {
                    reverse = "시작-while문";//평행사변형
                }
                else if (result == "if")
                {
                    reverse = "시작-if문";//마름모
                }
                else if (result == "elif")
                {
                    reverse = "시작-elif 문";//마름모
                }
                else if (result.Contains("else"))
                {
                    reverse = "시작-else 문";//마름모
                }
                else if (result == "def")
                {
                    reverse = "시작-def 문";
                }
                else if (result == "with")
                {
                    reverse = "시작-with 문";
                }
                else if (result.Contains("print("))
                {
                    reverse = "print 문";//물결
                }
                return reverse;
            }

            string Text_C = "";
            string Text_changed = "";
            bool check_anno = false; // 주석인지 확인
            bool check_import = false; // import문인지 확인
            bool check_def = false; // def인지 확인
            bool check_end = false; // \t 종료 확인
            string[] code = Text.Split('\n'); // 줄바꿈을 기준으로 split
            string[] code_reverse = new string[code.Length];

            List<string> tmp_if = new List<string>();
            Stack<string> stack = new Stack<string>();//조건문 입력,종료 표기할 스택
            Stack<int> stack_lenght = new Stack<int>();// 들여쓰기 개수 스택
            //Dictionary<string, int> dic = new Dictionary<string, int>(); //딕셔너리 입력,종료 표기할 스택
            for (int i = 0; i < code.Length; i++) // 줄의 길이만큼 반복
            {
                string[] code_re = code[i].Split(' '); // 공백을 기준으로 split
                //Console.WriteLine(code[i]);
                string result = code_re[0]; // 가장 앞
                //Console.WriteLine(result);
                List<string> tmp = new List<string>(code_re);

                string reverse = "";
                int j = 0;

                while (result == "tab") // \t일 경우 
                {
                    tmp.RemoveAt(j); // "tab"제거
                    reverse = Code(result);
                    tmp.Insert(j, reverse); // 다시 공백 넣어줌
                    j++; // 공백 개수
                    result = code_re[j]; // 가장 앞 단어 변경
                    code_re = tmp.ToArray();
                }
                //if (result == "\r\n") //tmp.Count == 0
                //{
                //    Console.WriteLine("빈줄");
                //    continue;
                //}///
                if (result == "import" || result == "from")
                {
                    continue;
                }
                if (check_anno == true) // 주석시 해당 문장 넘김
                {
                    string lastVal = tmp[tmp.Count - 1]; // 가장 마지막 단어
                    if (lastVal.Contains("'''"))
                    {
                        check_anno = false;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (result.Contains("#"))
                {
                    continue;
                }
                else if (result.Contains("'''"))
                {
                    check_anno = true;
                    continue;
                }
                //else if( )
                //{

                //}
                for (int a = j; a < tmp.Count; a++)
                { // 중간 주석 처리
                    string annotation = tmp[a];
                    if (annotation.Contains("#"))
                    {
                        tmp.RemoveRange(a, tmp.Count - a);
                        code_re = tmp.ToArray();
                    }
                }

                if (result == "for") // : 붙은경우 안붙은경우 구별 후 range, enumerate 처리
                {
                    string lastVal = tmp[tmp.Count - 1]; // 마지막 단어가져옴
                    if (lastVal == ":") // : 안붙은경우
                    {
                        tmp.RemoveAt(tmp.Count - 1); // : 제거
                    }
                    else if (code_re[j + 2].Contains("enumerate(")) //? : 붙은 경우 - enumerate// (, [ 
                    {
                        int c = lastVal.Length - 3;
                        lastVal = lastVal.Substring(11, c); // enumerate(, ): 제거
                        Console.WriteLine(lastVal);
                    }
                    else if (code_re[j + 2].Contains("range(")) // : 붙은 경우 - range
                    {
                        int c = lastVal.Length - 2;
                        lastVal = lastVal.Substring(7, c); // range(, ): 제거
                        //Console.WriteLine(lastVal);
                    }
                    else // 일반 리스트인 경우
                    {
                        int l = lastVal.Length - 2;
                        lastVal = lastVal.Substring(0, l);
                    }
                    tmp.RemoveAt(j); // for 제거
                    tmp.RemoveAt(j + 1); //? in 제거 // 값이 여러개인경우 for i, letter in enumerate(['A', 'B', 'C'], start=1):


                    tmp[tmp.Count - 1] = lastVal;
                    reverse = Code(result);
                    tmp.Insert(j, reverse);
                    code_re = tmp.ToArray();
                    //dic.Add(reverse, j+1);
                    reverse = reverse.Substring(3,reverse.Length-4);
                    reverse = "종료-" + reverse;
                    stack.Push(reverse);
                    stack_lenght.Push(j);
                    check_end = true;
                }
                else if (result == "if" || result.Contains("else") || result == "elif" || result == "while" || result == "def" || result == "with")
                {
                    //Console.WriteLine("if조건문들어옴");
                    string lastVal = tmp[tmp.Count - 1];

                    if (lastVal == ":") // : 안붙은경우
                    {
                        Console.WriteLine(":안붙은경우");
                        Console.WriteLine(lastVal);
                        tmp.RemoveAt(tmp.Count - 1); // : 제거
                    }
                    else if (result.Contains("else"))
                    {
                        lastVal = tmp[tmp.Count - 2];
                        int c = lastVal.Length - 1;
                        lastVal = lastVal.Substring(0, c);
                        tmp[tmp.Count - 1] = lastVal;
                    }
                    else // : 붙은경우
                    {
                        //Console.WriteLine(":붙은경우");
                        if (lastVal == " ")
                        {
                            //Console.WriteLine("공백 포함");
                            lastVal = tmp[tmp.Count - 2]; // 가장 마지막 리스트 가져와서
                            //Console.WriteLine(lastVal);
                            lastVal = lastVal.Substring(0, lastVal.Length - 1); // 가장 뒤의 : 제거 후
                            //tmp.RemoveAt(tmp.Count - 1);
                            tmp[tmp.Count - 2] = lastVal;  //그 자리에 제거한 리스트 추가
                        }
                        else
                        {
                            //Console.WriteLine("공백 미포함");
                            //Console.WriteLine(lastVal);
                            lastVal = lastVal.Substring(0, lastVal.Length - 2); // 가장 뒤의 : 제거 후
                            //tmp.RemoveAt(tmp.Count - 1);
                            tmp[tmp.Count - 1] = lastVal;
                        }
                        //Console.WriteLine(lastVal);
                        ////lastVal = tmp[tmp.Count - 2];// 가장 마지막 리스트 가져와서
                        //lastVal = lastVal.Substring(0, lastVal.Length - 1); // 가장 뒤의 : 제거 후
                        ////tmp.RemoveAt(tmp.Count - 1); // :가 붙어있는 마지막 리스트 삭제
                        //tmp[tmp.Count - 1] = lastVal; //그자리에 제거한 리스트 추가
                    }
                    reverse = Code(result);
                    tmp.RemoveAt(j);
                    tmp.Insert(j, reverse);
                    code_re = tmp.ToArray();
                    //dic.Add(reverse, j+1);
                    reverse = reverse.Substring(3, reverse.Length - 4);
                    reverse = "종료-" + reverse;
                    stack.Push(reverse);
                    stack_lenght.Push(j);
                    check_end = true;
                }
                else if (result.Contains("print("))
                {
                    reverse = Code(result); // print.문 변환
                    tmp.RemoveAt(j);
                    tmp.Insert(j, reverse); //변환 print문
                    if (result.Contains("print(") && result.Contains(")")) // 한개의 변수 출력일 경우
                    {
                        Console.WriteLine("print안");
                        //result = result.Substring(6, result.Length - 6);
                        result = result.Substring(6, result.Length - 8);
                        tmp.Insert(j + 1, result);
                    }
                    else
                    {
                        result = result.Substring(6, result.Length - 6); //"print("제거
                        string lastVal = tmp[tmp.Count - 1]; // 마지막 단어가져옴
                        lastVal = lastVal.Substring(0, lastVal.Length - 2); //")"제거
                        tmp.Insert(j + 1, result); //자른 앞print문
                        tmp.RemoveAt(tmp.Count - 1);
                        tmp.Insert(tmp.Count, lastVal); //자른 뒤print문
                    }
                    code_re = tmp.ToArray();
                }
                //else
                //{

                //}
                //// 다음 조건절 처리
                int k = 0;
                if (i < code.Length - 1)
                { // 마지막 문장 전까지 실행
                    int l;
                    j = 0;
                    bool check_anno_next = false;
                    for (l = i + 1; l < code.Length; l++) // 줄의 길이만큼 반복
                    {
                        while (result == "tab") // \t일 경우 공백처리
                        {
                            tmp.RemoveAt(j);
                            reverse = Code(result);
                            tmp.Insert(j, reverse); // 다시 공백 넣어줌
                            j++; // 공백 개수
                            result = code_re[j]; // 가장 앞 단어 변경
                            code_re = tmp.ToArray();
                        }
                        if (result == " ")
                        {
                            continue;
                        }///
                        if (check_anno_next == true) // 주석시 해당 문장 넘김
                        {
                            string lastVal = tmp[tmp.Count - 1]; // 가장 마지막 단어
                            if (lastVal.Contains("'''")) // 여러줄 주석일경우
                            {
                                check_anno_next = false; // 넘겨야함을 표시
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else if (result.Contains("#"))
                        {
                            continue;
                        }
                        else if (result.Contains("'''"))
                        {
                            check_anno_next = true;
                            continue;
                        }
                        else
                        {
                            break;
                        }

                    }
                    Console.WriteLine(l);
                    string[] next_code = code[l].Split(' '); // 다음줄 공백을 기준으로 split
                    string next_result = next_code[0]; // 다음문장의 가장 앞 단어                   

                    List<string> tmp_2 = new List<string>(next_code);
                    if (next_result == "tab") // 다음 문장이 \t된 경우
                    {
                        while (next_result == "tab") // \t 모두 제거 
                        {
                            tmp_2.RemoveAt(k); // 가장 앞 tab 제거
                            reverse = Code(next_result); // 공백으로 대체
                            tmp_2.Insert(k, reverse); // 다시 공백 넣어줌
                            k++; // 공백개수
                            next_result = next_code[k]; // 가장 앞 단어 변경
                            //next_code = tmp_2.ToArray();
                        }
                        Console.WriteLine("공백개수 : " + k);
                    }

                    check_end = true;
                    try
                    {
                        //Console.WriteLine("스택에 있는 공백길이: "+stack_lenght.Peek());
                        while ((check_end == true && stack_lenght.Peek() == k) || stack_lenght.Peek() > k) // 다음줄의 공백이 현재 조건문의 공백과 같다면 
                        {
                            string lastVal = tmp[tmp.Count - 1]; // 가장 마지막 단어
                            String stack_str = stack.Pop();
                            stack_lenght.Pop();
                            tmp.Insert(tmp.Count, stack_str);
                            code_re = tmp.ToArray();
                        }
                    }
                    catch (Exception e)
                    {
                        check_end = false;
                    }
                    check_end = false;
                }
                else
                { // 마지막 문장일 경우
                    while (stack.Count != 0)
                    {
                        string lastVal = tmp[tmp.Count - 1]; // 가장 마지막 단어
                        String stack_str = stack.Pop();
                        stack_lenght.Pop();
                        tmp.Insert(tmp.Count, stack_str);
                        code_re = tmp.ToArray();
                    }

                }
                Text_C = string.Join(" ", code_re);
                Console.WriteLine(Text_C);
                code_reverse[i] = Text_C;
            }
            Text_changed = string.Join("\r\n", code_reverse); //\n - 메모장, \r\n - 텍스트박스
            return Text_changed;
        }
        private string Structure_Re(string theText2)
        {
            string Text_C = "";
            string Text_changed = "";
            bool check_anno = false; // 주석인지 확인
            bool check_import = false; // import문인지 확인
            bool check_def = false; // def인지 확인
            bool check_end = false; // \t 종료 확인
            string[] code = theText2.Split('\n'); // 줄바꿈을 기준으로 split
            string[] code_reverse = new string[code.Length];

            string Structure_changed = "";
            string Conditional = "";

            List<string> tmp_if = new List<string>();
            Stack<string> stack = new Stack<string>();//조건문 입력,종료 표기할 스택
            Stack<int> stack_lenght = new Stack<int>();// 들여쓰기 개수 스택
            
            for (int i = 0; i < code.Length; i++) // 줄의 길이만큼 반복
            {
                string[] code_re = code[i].Split(' '); // 공백을 기준으로 split
                //Console.WriteLine(code[i]);
                string result = code_re[0]; // 가장 앞
                //Console.WriteLine(result);
                List<string> tmp = new List<string>(code_re);
                
                string reverse = "";
                if (result.Contains("시작-"))
                {
                    Conditional = result.Substring(2,result.Length - 3);
                    Console.WriteLine(Conditional);
                }


            }
            Structure_changed = string.Join("\r\n", code); 
            return Structure_changed;
        }
    }
}

//주석 처리 Ctrl + K +C , Ctrl + U +C
// : 두가지로 나눠서 구현
//for- range, enumerate ,def , 코드 중간 주석처리(추가로 : 뒤 주석처리일시)//

// 중간에  print()가 들어간 경우 프린트문으로 처리해버림

//------------------------------------------------------------------
// 다음줄이 들여쓰기 되어있다면, 
// 그 다음줄에 앞문장의 조건문 넣어주기? - 자를때 걸림 , 같은 조건문이 반복해서 나오면 구분이 힘듬

// 해당줄이 들여쓰기 되어있다면
// 이전줄의 조건문 넣어주기 - 어디까지 

// 다음줄이 들여쓰기라면 들여쓰기 개수를 읽어서 
// 들여쓰기 종료되는 부분에서 해당문 종료 입력

//주석처리를 먼저
//---------------------------------------------------------------------
//print 변수일경우 한단어라 마지막으로 다시 추가됨//
//공백이 있을때 print문 수정
//else문 종료, , elif :안잘림, 이중조건문 종료 
// from, import 처리 필요
// def 처리 필요 :붙은경우 안붙은 경우
//평문 중간주석처리
//빈문장일때 다음문장으로 체크하기
//조건문 글자 잘리는것
//
//-------------------------------해결----------------------------------------
///
// with 처리, class 처리
// for문 글자잘림 
// 조건문 중간에 or이나 and 등이 들어간 경우
// 조건문일때 구역에 맞게 따로 저장할수있도록?
//-------------------------------해결필요----------------------------------------
//                    //check_if = true;                    
//tmp.RemoveAt(tmp.Count - 1);
//if (result.Contains("else"))
//{
//    if(code_re[1] == "if") //else if라면
//    {
//        tmp.RemoveAt(0);
//        result = "else if";
//    }
//}
//
//---------------------------------------------------------------
//