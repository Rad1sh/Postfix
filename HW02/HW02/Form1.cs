using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace HW02
{

    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        

        public string input = Console.ReadLine();


        public static int ComputeInfix(string infix)
        {
            var operatorstack = new Stack<char>();
            var operandstack = new Stack<int>();

            var precedence = new Dictionary<char, int> { { '(', 0 }, { '*', 1 }, { '/', 1 }, { '+', 2 }, { '-', 2 }, { ')', 3 } };

            foreach (var ch in infix)
            {
                switch (ch)
                {
                    case var digit when Char.IsDigit(digit):
                        operandstack.Push(Convert.ToInt32(digit.ToString()));
                        break;
                    case var op when precedence.ContainsKey(op):
                        var keepLooping = true;
                        while (keepLooping && operatorstack.Count > 0 && precedence[ch] > precedence[operatorstack.Peek()])
                        {
                            switch (operatorstack.Peek())
                            {
                                case '+':
                                    operandstack.Push(operandstack.Pop() + operandstack.Pop());
                                    break;
                                case '-':
                                    operandstack.Push(-operandstack.Pop() + operandstack.Pop());
                                    break;
                                case '*':
                                    operandstack.Push(operandstack.Pop() * operandstack.Pop());
                                    break;
                                case '/':
                                    var divisor = operandstack.Pop();
                                    operandstack.Push(operandstack.Pop() / divisor);
                                    break;
                                case '(':
                                    keepLooping = false;
                                    break;
                            }
                            if (keepLooping)
                                operatorstack.Pop();
                        }
                        if (ch == ')')
                            operatorstack.Pop();
                        else
                            operatorstack.Push(ch);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            if (operatorstack.Count > 0 || operandstack.Count > 1)
                throw new ArgumentException();

            return operandstack.Pop();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string result = "";
           
            result = rec(e.Node, result);
            label1.Text = result + " = " + ComputeInfix(result);
        }

        public string rec(TreeNode ParentNode, string equation)
        {
   
            string eq = equation;
            TreeNode tempNode = ParentNode;


            int NodeCounter = 0;
            foreach (TreeNode currentNode in tempNode.Nodes)
                NodeCounter++;

            if(NodeCounter != 0)
            eq = "(" + eq;
            if (NodeCounter >= 1)
            {
                eq = rec(tempNode.Nodes[0],eq);
              
            }
     
            eq = eq + tempNode.Text;
           

            if (NodeCounter >= 2)
            {
                eq = rec(tempNode.Nodes[1], eq);
            
            }
            if (NodeCounter != 0)
                eq = eq + ")";

           
            return eq;
        }

        public int calculate(int a, int b ,string op)
        {
            if (op == "+")
                return a + b;
            else if (op == "-")
                return a - b;
            else if (op == "*")
                return a * b;
            else if (op == "/")
                return a / b;
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            


            Node rootNode = null;
            Tree tree = new Tree();


            for (int i = input.Length - 1; i >= 0; i--)
            {
                rootNode = tree.insert(rootNode, input[i]);

            }


            TreeNode tNode = new TreeNode();
            // TreeView t2 = new TreeView();


            tNode.Nodes.Add("CALCULATOR");
            //tNode.Nodes[0].Nodes.Add("root_directory 2");


            TreeNode resultNode = tree.recursiveNode(rootNode, tNode.Nodes[0]);

            //resultNode.Nodes.Remove(resultNode.FirstNode);


            treeView1.Nodes.Add(resultNode.Nodes[0]);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }

    class Node
    {
        public char value = ' ';
        public Node left = null;
        public Node right = null;
    }

    class Tree
    {
        bool isAdded = false;
        public Node insert(Node root, char v)
        {
            isAdded = false;
            if (root == null)
            {
                Console.WriteLine("Initial Insert " + v + "\n----------\n");
                root = new Node();
                root.value = v;
                isAdded = true;
            }
            else
            {
                if (isOperator(root.value) == true && isAdded == false)
                {
                    Console.WriteLine("go Left " + root.value);
                    root.left = insert(root.left, v);
                }
                if (isOperator(root.value) == true && isAdded == false)
                {
                    Console.WriteLine("go Right " + root.value);
                    root.right = insert(root.right, v);
                }
                if (isAdded == false)
                {
                    Console.WriteLine("back to parent");
                }
            }
            return root;
        }


        public TreeNode recursiveNode(Node parent, TreeNode tree)
        {

            Node temp = parent;
            TreeNode tempTree = tree;
            tempTree.Nodes.Add(temp.value.ToString());
            //tempTree.Text = temp.value+"";

            if (temp.left != null)
            {
               
                TreeNode tm = recursiveNode(temp.left, tempTree.LastNode);
                tempTree.Nodes.Remove(tempTree.LastNode);
                tempTree.Nodes.Add(tm);
                


            }
            if (temp.right != null)
            {
               
                TreeNode tm = recursiveNode(temp.right, tempTree.LastNode);
                tempTree.Nodes.Remove(tempTree.LastNode);
                tempTree.Nodes.Add(tm);



            }

           


            return tempTree;

            
        }

        public static bool isNumber(char n)
        {
            if (n >= '0' && n <= '9')
                return true;
            else
                return false;
        }
        public static bool isOperator(char n)
        {
            if (n == '+' || n == '-' || n == '*' || n == '/')
                return true;
            else
                return false;
        }
    }


}
