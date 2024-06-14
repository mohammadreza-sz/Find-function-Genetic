using System;
using System.Text;
using System.Collections.Generic;
namespace my_project
{

    using System.Threading;
    class program
    {
        static void swap(ref double a, ref double b)
        {
            double c = 0;
            c = a;
            a = b;
            b = c;
        }
        class priority_queue_min_heap_based1
        {
            double[ , ] Array;
            long total_size;
            public long current_size;
            public priority_queue_min_heap_based1(long size)
            {
                Array = new double[size + 1 , 2];

                this.total_size = size;
            }
            long parent(long i)
            {
                return i / 2;
            }
            long left_child(long i)
            {
                return 2 * i;
            }
            long right_child(long i)
            {
                return 2 * i + 1;
            }
            void sift_up(long i)
            {

                while (i > 1 && Array[parent(i),0] > Array[i,0])
                // while (i > 1 && Array[parent(i),0] < Array[i,0])
                {
                    long p = parent(i);
                    swap(ref Array[p,0], ref Array[i,0]);
                    swap(ref Array[p, 1], ref Array[i, 1]);
                    i = p;
                }

            }
            void sift_down(long i)
            {

                long min_index = i;
                long l = left_child(i);

                if (l <= current_size && Array[l , 0] < Array[min_index , 0])
                // if (l <= current_size && Array[l , 0] > Array[min_index , 0])
                {
                    min_index = l;
                }

                long r = right_child(i);
                if (r <= current_size && Array[r , 0] < Array[min_index , 0])
                // if (r <= current_size && Array[r , 0] > Array[min_index , 0])
                {
                    min_index = r;
                }
                if (min_index != i)
                {
                    swap(ref Array[i,0], ref Array[min_index,0]);
                    swap(ref Array[i, 1], ref Array[min_index, 1]);
                    sift_down(min_index);
                }
            }
            public void insert(double key, double which_vertex)
            {
                if (current_size == total_size)
                    return;
                current_size = current_size + 1;
                Array[current_size , 0] = key;
                Array[current_size, 1] = which_vertex;
                sift_up(current_size);
            }
            /// <summary>
            /// agar khoroji array bashe index aval mse va index dovom shomare derakht
            /// </summary>
            public double[] Extract_index_of_min()
            {
                double []result =new double[2];
                result[0] = Array[1,0];
                result[1]= Array[1,1];

                // double result;
                // result = Array[1,1];

                Array[1,0] = Array[current_size,0];
                Array[1, 1] = Array[current_size, 1];

                current_size = current_size - 1;

                Array[current_size + 1,0] = 0;
                Array[current_size + 1, 1] = 0;

                sift_down(1);
                return result;
            }
            public void remove(long i)
            {
                Array[i,0] = double.MinValue;
                sift_up(i);
                Extract_index_of_min();
            }
            public bool is_empty()
            {
                if (current_size == 0)
                    return true;
                return false;
            }
        }
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static int rnd_seed = 0;
        // static Random rnd = new Random(Guid.NewGuid().GetHashCode());
        
        const int limit_of_operator = 7 , limit_for_depth =15 , limit_for_constant = 500 , limit_for_type = 3 ;
        const int test_case =200, maximum_variable_value = 100 , minumum_variable_value = -100 ;
        static  int tree_instance_count = 1000 ;
        const int last_generation_percent = 30, worst_mse_percent = 50 ;//,best_mse_percent = 60 ;
        const double expected_mse =0.00001 ;
        
        public enum type
        {
            op,
            constant,
            variable
        }
        static double calculate_function_value(ref int variable_value)
        {
            check:
            // double res = (double)(variable_value*5/3)-100;//(x*5/3)-100
            // double res = (double)(Math.Sin(variable_value) + Math.Cos(variable_value))*variable_value*variable_value;
            // double res = Math.Sin(variable_value*variable_value + 100);
            // double res = Math.Pow((double)variable_value , 5)+Math.Pow((double)variable_value , 3) - 167;
            // double res = variable_value*variable_value*variable_value + variable_value;
            double res = (Double)((variable_value*variable_value)+(variable_value-1)+10)/((variable_value*variable_value)+(variable_value-5));

            // double res  = (double) ((variable_value*variable_value)+(3*variable_value)-Math.Sin(variable_value))/(2*variable_value*variable_value*variable_value);

            // double res = variable_value*variable_value*variable_value + variable_value * variable_value*Math.Tan(variable_value);
            if(double.IsNaN(res) == true)
            {
                variable_value = rnd.Next(minumum_variable_value , maximum_variable_value);
                goto check;
            }
            return res;
            
        }

        public class NODE
        {
            public type t;
            public string value;
            public NODE parent;
            // public bool binary_opertaor = true;
            public bool binary_opertaor;
            public NODE right;
            public NODE left;
            // public NODE(string value , type t , NODE parent = null , NODE right = null , NODE left = null)
            // {
            //     this.value = value;
            //     this.t = t;
            //     this.parent = parent;
            //     this.left = left;
            //     this.right = right;

            // }
            public  NODE (){

            }
            public NODE(string value , type t , int which_operator,NODE parent = null)
            {
                this.value = value;
                this.t = t;
                this.parent = parent;
                if(which_operator >= 5)
                    binary_opertaor = false;
                else 
                    binary_opertaor = true;
                
            }
            /// <summary>
            /// entekhab ezafe kardan node seda shode
            /// </summary>
            /// <param name="which">node left ya right ro be sorat tasadofi por mikone </param>
            /// <param name="x">agar sefr bashe ye adad bein 1 va limit_for_type entekhab mishe ke malom mikone node bayad chi bashe </param>
            // public void fill_randomly(string which , int x = 0){
                public void fill_randomly(int x = 0 , bool if_jump = false)
                {//////////////////////////////////
                if(x == 0)
                {
                    // rnd = new Random(rnd_seed++);
                    x = rnd.Next(1 , limit_for_type + 1);
                }

                switch (x)
                {
                    case 1:
                        this.value = rnd.Next(1 , limit_for_constant + 1).ToString();
                        // this.binary_opertaor=?
                        this.t = type.constant;                        
                        break;
                    case 2:
                        this.value = "x";
                        this.t = type.variable;
                        break;
                    case 3:
                        x = rnd.Next(1 , limit_of_operator + 1);
                        this.value = generate_operator(x);
                        if(if_jump == true)
                        {
                            if(this.binary_opertaor == false && x < 5)
                            {
                                this.left = new NODE();
                                this.left.fill_randomly(rnd.Next(1 , 3));
                            }
                        }

                        if(x< 5)
                            this.binary_opertaor = true;
                        else
                            this.binary_opertaor = false;
                        

                        this.t = type.op;
                        break;
                    default:
                        break;
                }
                
            }
        }
        public class tree
        {
            public NODE root;
            public tree(NODE root)
            {
                this.root = root;
            }
            public tree()
            {
                //root must be operator
                int random = rnd.Next(1 , limit_of_operator + 1);
                root = new NODE(generate_operator(random) , type.op , random);
            }
            public void fill_tree_randomly( )//////////////////////////////////
            {
                
                NODE current = root;
                Stack<NODE> stack_node = new Stack<NODE>();
                stack_node.Push(current);
                while(stack_node.Count() != 0)
                {
                    if(current.t == type.op /* || current.t == type.u_op unary operator*/)
                    {
                        if(current.binary_opertaor == true)
                        {
                            if(stack_node.Count() == limit_for_depth) //if reach to limit of depth
                            {
                                // current.left = new NODE("" , type.)
                                current.left = new NODE();
                                current.right= new NODE();
                                current.left.fill_randomly( rnd.Next(1 , 3));//get random number because must be constant or variable
                                current.right.fill_randomly(rnd.Next(1 , 3));
                                current = stack_node.Pop();
                                continue;
                            }
                            
                            if(current.left ==null)
                            {
                                stack_node.Push(current);
                                
                                NODE new_node = new NODE();
                                current.left = new_node;
                                new_node.fill_randomly();
                                // current.fill_randomly("left");
                                current = current.left;
                                continue;
                            }
                            if(current.right == null)
                            {
                                stack_node.Push(current);
                                NODE new_node = new NODE();
                                current.right = new_node;
                                new_node.fill_randomly();
                                // current.fill_randomly("right");
                                current = current.right;
                                continue;
                            }
                            current = stack_node.Pop();
                            continue;

                        }
                        //************TODO for unary operator
                        else{
                            if(stack_node.Count() == limit_for_depth) //if reach to limit of depth
                            {
                                // current.left = new NODE("" , type.)
                                // current.left = new NODE();
                                current.right= new NODE();
                                // current.left.fill_randomly( rnd.Next(1 , 3));
                                current.right.fill_randomly(rnd.Next(1 , 3));
                                current = stack_node.Pop();
                                continue;
                            }
                            

                            if(current.right == null)
                            {
                                stack_node.Push(current);
                                NODE new_node = new NODE();
                                current.right = new_node;
                                new_node.fill_randomly();
                                // current.fill_randomly("right");
                                current = current.right;
                                continue;
                            }
                            current = stack_node.Pop();
                            continue;
                        }
                    }
                    else
                    {
                        current = stack_node.Pop();
                        continue;
                    }
                }
            }           

        }
        static StringBuilder res = new StringBuilder("");
        public static StringBuilder write_tree(NODE root , bool x = false)//////////////////////////////////
        {
            if(x == true)
                res = new StringBuilder("");

            if(root.left != null)
            {
                res.Append("(");
                // Console.Write("(");
                write_tree(root.left);
            }

            // Console.Write(root.value);
            res.Append(root.value);

            if(root.right != null)
            {
                if(root.binary_opertaor == false)
                    res.Append("(");
                write_tree(root.right);
                res.Append(")");
                // Console.Write(")");
            }
            return res;
            
        }
        /// <summary>
        /// az bein operator haye mojod ba tavajoh be vorodi yekisho barmigardone
        /// </summary>
        /// <param name=""></param>
        public static string generate_operator(int which)
        {//change for operator
            switch (which)
            {
                case 1:
                    return "+";
                case 2:
                    return "-";
                case 3:
                    return "*";
                case 4:
                    return "/";
                case 5:
                    return "sin";
                case 6:
                    return "cos";
                case 7:
                    return "tan";
                case 8:
                    return "cot";
                default:
                    return "+";/// TODO: /////////////////////////////////////////////////////////////
            }
        }
        public static string calculate_tree_value(NODE node, string variable_value)//////////////////////////////////
        {
            if(node.t ==type.constant)
            {
                return node.value;
            }
            if(node.t == type.variable)
            {
                return variable_value;
            }
            if(node.binary_opertaor == false) //todo for unary operator
                return calculate_operation_value("+" , node.value , calculate_tree_value(node.right , variable_value));
            else
                return calculate_operation_value(calculate_tree_value(node.left , variable_value) , node.value , calculate_tree_value(node.right , variable_value));
        }
        public static string calculate_operation_value(string left , string opereation , string right)
        {
            switch (opereation)
            {
                //change for operator
                case "+":
                    return (double.Parse(left) + double.Parse(right)).ToString();
                case "-":
                    return (double.Parse(left) - double.Parse(right)).ToString();
                case "*":
                    return (double.Parse(left) * double.Parse(right)).ToString();
                case "/":
                    return ((double)double.Parse(left) / double.Parse(right)).ToString();
                case "sin":
                    return (Math.Sin(double.Parse(right)).ToString());
                case "cos":
                    return (Math.Cos(double.Parse(right)).ToString());
                case "tan":
                    return (Math.Tan(double.Parse(right)).ToString());
                case "cot":
                    return (((double)1/Math.Tan(double.Parse(right))).ToString());
                default:
                    return "";
            }
            
        }
        static double mse( NODE t , ref double[] function_value , ref int[] variable_value)//////////////////////////////////
        {
            double size =function_value.Count() ,  sum = 0 , cal , res;;
            string cal2;
            for (var i = 0; i <function_value.Count() ; i++)
            {
                // cal2= calculate_tree_value(t.root , variable_value[i].ToString());
                cal2= calculate_tree_value(t, variable_value[i].ToString());
                

                if(cal2 == "NaN")
                {
                    size--;
                    continue;
                }
                
                cal = double.Parse(cal2);
                res = Math.Pow(cal-function_value[i] , 2);
                sum +=(res);
            }
            return (sum/size);
        }
        /// <summary>
        /// miad be sorat tasadofi type hamon node ro taghir mide
        /// </summary>
        static void jump(ref NODE node)//////////////////////////////////
        {
            if(node.t == type.op)
                
                node.fill_randomly(3 , true);
            else
                node.fill_randomly(rnd.Next(1 , 3));

        }
        static int depth ;
        /// <summary>
        /// miad root ro migire va yeki az node hasho be sorat tasadofi barmigardone 
        /// </summary>
        // static NODE generation(tree t1 )
        static NODE get_random_node(NODE t1)
        {
            int rand;
            depth=0;
            // NODE current = t1.root;
            NODE current = t1;
            while(current.t == type.op)
            {
                depth++;
                if(current.binary_opertaor == true)
                    rand = rnd.Next(1 , 7);
                else
                    rand = rnd.Next(2 , 7);
                switch (rand)//age unary beseh niaz be update dare
                {
                    case 1:
                        current = current.left;
                        continue;
                    case 2 :
                        current = current.right;
                        continue;
                    case 3 or 4 or 5 or 6:
                        // if(rnd.Next(1 , 100)== 50)
                        //     jump(ref current);
                        return current;
                }
            }
            // if(rnd.Next(1 , 100)== 50)
            //     jump(ref current);//jahesh
            return current;
        }
        static void mix(ref NODE random_node_from_parent ,ref NODE random_node_from_random_node )
        {
            if(random_node_from_parent.parent == null)
            {
                random_node_from_parent = random_node_from_random_node;
            }
            else{
                random_node_from_random_node.parent = random_node_from_parent.parent;

                if(random_node_from_parent.parent.left ==random_node_from_parent){
                    random_node_from_parent.parent.left = random_node_from_random_node;
                }
                else
                    random_node_from_parent.parent.right = random_node_from_random_node;
            }
        }
        /// <summary>
        /// get two root and swap (mix) two nodes(modify the node) and return new node with probability of jump
        /// </summary>
        static NODE generate_new_node(NODE parent_node , NODE  random_node)
        {
            again://depth nabayad ziad beshe
            NODE random_node_from_parent = get_random_node(parent_node);
            int parent_depth = depth;
            NODE random_node_from_random_node= get_random_node(random_node);
            max_depth(random_node_from_random_node);
            max_depht = 0;
            int random_node_max_depth =max_depht;
            if(parent_depth + random_node_max_depth >= 25)
                goto again;
            mix(ref random_node_from_parent , ref random_node_from_random_node);
            if(rnd.Next(1 , 100) == 50)
            {
                NODE x = get_random_node( random_node_from_parent);
                jump(ref x);
            }
            return random_node_from_parent;
        }
        static int max_depht;
        static void max_depth(NODE cur , int max = 0)
        {            
            if(max > max_depht)
                max_depht = max;
            if(cur.left != null)
            {
                max++;
                max_depth(cur.left , max);
                max--;
            }
            if(cur.right != null)
            {
                max++;
                max_depth(cur.right , max);
                max--;
            }
        }
        static void generation(ref tree[] trees , ref priority_queue_min_heap_based1 pmax)
        {            


            int present_node_count = trees.Count() * last_generation_percent / 100;//(best_mse_percent + worst_mse_percent) / 100;
            int bad_generation = (trees.Count() - present_node_count) * worst_mse_percent/ 100;
            int good_generation =trees.Count() - present_node_count - bad_generation;
            int random;
            // int good_generation =(trees.Count() - present_node_count)*best_mse_percent/100;//inja saghf begirim
            // for (var i = present_node_count; i < present_node_count +good_generation; i++)
            for (var i = present_node_count; i < present_node_count +present_node_count; i++)
            {
                random = rnd.Next(0 , present_node_count);
                NODE parent_node = copy(trees[random].root);
                // Console.WriteLine("parent node from trees[{0}] is:" , random);
                // Console.WriteLine(write_tree(parent_node , true));

                random = rnd.Next(0 , present_node_count);
                NODE random_node = copy(trees[random].root);
                // Console.WriteLine("random node from trees[{0}] is:", random);
                // Console.WriteLine(write_tree(random_node, true));

                NODE generated_node = generate_new_node(parent_node , random_node);

                
                
            }

            for (var i = present_node_count + good_generation; i < trees.Count();i++)//trees.count() - present_node_count +good_generation; i++)
            {
                random = rnd.Next(0 , present_node_count);
                NODE parent_node = copy(trees[random].root);
                // Console.WriteLine("parent node from trees[{0}] is:" , random);
                // Console.WriteLine(write_tree(parent_node , true));

                // random = rnd.Next(trees.Count() - bad_generation , trees.Count());
                random = rnd.Next(present_node_count , trees.Count());                
                NODE random_node = copy(trees[random].root);
                // Console.WriteLine("random node from trees[{0}] is:", random);
                // Console.WriteLine(write_tree(random_node, true));

                NODE generated_node = generate_new_node(parent_node , random_node);
                trees[i].root =generated_node;
            
            }

          
        }
        static NODE copy(NODE node1)
        {
            NODE node2 = new NODE();
            node2.t = node1.t;
            node2.binary_opertaor = node1.binary_opertaor;
            node2.value = node1.value;
            // node2.right = node1.right;
            // node2.left = node1.left;
            // // node2.parent = node1.parent;
            if(node1.left != null)
            {
                node2.left = copy(node1.left);
                node2.left.parent = node2;
            }
            if(node1.right != null)
            {
                node2.right = copy(node1.right);
                node2.right.parent = node2;
            }
            return node2;
        }

        static List<NODE> temp_root_node = new List<NODE>();
        static void Main(string[]args)
        {
            for (int ii = 0; ii < 1; ii++)
            {
            DateTime t1 = DateTime.Now , t2 = DateTime.Now; // assigns default value 01/01/0001 00:00:00
     
            tree[] trees = new tree[tree_instance_count];                       
            for (int i = 0; i < tree_instance_count; i++)
            {
                trees[i] = new tree();
                // Thread.Sleep(2000);
            }
            for (var i = 0; i < tree_instance_count; i++)
            {
                trees[i].fill_tree_randomly();

            }

            int []variable_value = new int[test_case];
            for (var i = 0; i < test_case; i++)
            {
                // variable_value[i] = rnd.Next(minumum_variable_value, maximum_variable_value);

                variable_value[i] = minumum_variable_value + i;
                // variable_value[i] = Math.Round(rnd.NextDouble() * rnd.Next(minumum_variable_value , maximum_variable_value) , 2);

            }

            double []function_value = new double[test_case];
            for (var i = 0; i < test_case; i++)
            {
                function_value[i] = calculate_function_value(ref variable_value[i]);
                // if(function_value[i] == double.NaN){
                //     function_value[i--] = rnd.Next(minumum_variable_value , maximum_variable_value);
                //     continue;
                // }

            }
            priority_queue_min_heap_based1 pmax = new priority_queue_min_heap_based1(2* tree_instance_count);
            // int counter=0;

            // double[] dp_mse= new double[tree_instance_count];
            // for (var i = 0; i < tree_instance_count; i++)
            // {
            //     dp_mse[i] = -1;   
            // }
            for (var ix = 0; ix < 1000; ix++)
            {
                Console.Clear();
                Console.WriteLine("in {0} step" , ix);
                if(ix== 800)
                {
                    ix++;
                    ix--;
                }
                double temp = -1;
                for (var i = 0; i < tree_instance_count; i++)
                {
                    
                    
                        temp = mse(/* ref */ trees[i].root , ref function_value , ref variable_value );
                        // dp_mse[i] = temp;

                    if(temp <expected_mse /* && temp>= 0 */)
                    {
                        t2 = DateTime.Now;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("find best function with mse: {0} in {1} step with duration :{2}" , temp , ix , t2 - t1);
                        Console.WriteLine( write_tree(trees[i].root, true) );
                        Console.ForegroundColor = ConsoleColor.White;
                        goto master_loop;
                        // return ;
                    }
                    pmax.insert( temp, i);
                }
                for (var i = 0; i < temp_root_node.Count(); i++)
                {
                    temp = mse(temp_root_node.ElementAt(i), ref function_value , ref variable_value );
                        // dp_mse[i] = temp;

                    if(temp <expected_mse /* && temp>= 0 */)
                    {
                        t2 = DateTime.Now;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("find best function with mse: {0} in {1} step with duration :{2}" , temp , ix , t2 - t1);
                        Console.WriteLine( write_tree(temp_root_node.ElementAt(i), true) );
                        Console.ForegroundColor = ConsoleColor.White;
                        goto master_loop;
                        // return ;
                    }
                    pmax.insert( temp, i+tree_instance_count);
                    
                }
                tree []temp_tree = new tree[tree_instance_count];
                string tree_result_pre = null, tree_result_new;
                double []saver = new double[2];
                int counter = 0;
                for (var i = 0; i < tree_instance_count; i++)
                {   
                    saver = pmax.Extract_index_of_min();

                    
                    if(saver[1]< tree_instance_count){//agar bargharar bashe yani jozve temp_root_node nist
                    // res = new StringBuilder("");
                    tree_result_new = write_tree(trees[(int)saver[1]].root , true).ToString();

                    
                    
                        if(tree_result_new == tree_result_pre)
                        {
                            // tree_instance_count--;
                            // i--;

                            // trees[(int)saver[1]]= new tree();
                            // trees[(int)saver[1]].fill_tree_randomly();
                            // tree_result_new = write_tree(trees[(int)saver[1]].root , true).ToString();

                            continue;

                            // continue;
                        }
                        else
                            tree_result_pre = tree_result_new;


                    // tree_result_pre = tree_result_new;
                    temp_tree[counter++] = trees[(int)saver[1]];
                    // temp_tree[i] = trees[(int)pmax.Extract_index_of_min()[1]];
                    }
                    else{
                        tree_result_new = write_tree(temp_root_node.ElementAt((int)saver[1]% tree_instance_count), true).ToString();

                    
                    
                        if(tree_result_new == tree_result_pre)
                        {
                            // tree_instance_count--;
                            // i--;

                            // trees[(int)saver[1]]= new tree();
                            // trees[(int)saver[1]].fill_tree_randomly();
                            // tree_result_new = write_tree(trees[(int)saver[1]].root , true).ToString();

                            continue;
                        }
                        else
                            tree_result_pre = tree_result_new;

                    temp_tree[counter++].root =temp_root_node.ElementAt((int)saver[1] % tree_instance_count);
                    }
                }
                if(counter != tree_instance_count)//agar bekhatere moshabeh ha kamel por nashod pas akharesh ro be sorat random por mikonim
                {
                    int size = tree_instance_count - counter;
                    for (var i = 0; i <size; i++)
                    {
                        temp_tree[counter] = new tree();
                        temp_tree[counter++].fill_tree_randomly();
                    }
                }
                pmax.current_size = 0;//make empty the binary heap
                temp_root_node.Clear();

                trees =temp_tree;
                
                generation(ref trees , ref pmax);
            }

            for (var i = 0; i < tree_instance_count; i++)
            {
                double temp = mse(/* ref */ trees[i].root , ref function_value , ref variable_value );
                // if(temp <= expected_mse)
                // {
                //     write_tree(trees[i].root);
                //     Console.WriteLine();
                //     return ;
                // }
                pmax.insert( temp, i);
            }
            double[] msee;
            // for (var i = 0; i < trees.Count(); i++)
            // {
                t2 = DateTime.Now; 

                msee = pmax.Extract_index_of_min();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("trees[{0}] with mse :{1} with time {2}" , msee[1] , msee[0] , t2 -t1);

                // res = new StringBuilder("");
                Console.WriteLine(write_tree(trees[(int)msee[1]].root , true));
                Console.ForegroundColor = ConsoleColor.White;
                master_loop:    
                Console.WriteLine();
            }    
            // }
        }
    }
}