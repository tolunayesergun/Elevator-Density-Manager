using ElevatorDensityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ElevatorDensityProject
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region GlobalDefines

        private readonly Random rnd = new Random();
        private Thread threadEntering;
        private Thread threadExit;
        private Thread threadControl;
        private Thread threadEle1;
        private Thread threadEle2;
        private Thread threadEle3;
        private Thread threadEle4;
        private Thread threadEle5;

        public static System.Drawing.Point[,] locationListEle = new[,]
            {
                    { new System.Drawing.Point(394, 452), new System.Drawing.Point(394, 342) , new System.Drawing.Point(394, 232) , new System.Drawing.Point(394, 122) , new System.Drawing.Point(394, 12) },
                    { new System.Drawing.Point(576, 452), new System.Drawing.Point(576, 342), new System.Drawing.Point(576, 232), new System.Drawing.Point(576, 122), new System.Drawing.Point(576, 12) },
                    { new System.Drawing.Point(752, 452), new System.Drawing.Point(752, 342), new System.Drawing.Point(752, 232), new System.Drawing.Point(752, 122), new System.Drawing.Point(752, 12) },
                    { new System.Drawing.Point(930, 452), new System.Drawing.Point(930, 342), new System.Drawing.Point(930, 232), new System.Drawing.Point(930, 122), new System.Drawing.Point(930, 12) },
                    { new System.Drawing.Point(1106, 452), new System.Drawing.Point(1106, 342), new System.Drawing.Point(1106, 232), new System.Drawing.Point(1106, 122), new System.Drawing.Point(1106, 12) },
              };

        private static readonly Lazy<List<Person>> lazy = new Lazy<List<Person>>(() => new List<Person>());

        public static List<Person> peopleList
        {
            get
            {
                return lazy.Value;
            }
        }

        private readonly List<Elevator> eleList = new List<Elevator>();

        private static int TotalPersonNumber = 0;

        #endregion GlobalDefines

        #region FormMethods

        private void MainForm_Load(object sender, EventArgs e)
        {
            GenerateElevator();
            GenerateThread();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (threadEntering != null)
            {
                threadEntering.Abort();
                threadExit.Abort();
                threadControl.Abort();
                threadEle1.Abort();
                threadEle2.Abort();
                threadEle3.Abort();
                threadEle4.Abort();
                threadEle5.Abort();
            }
        }

        private void GenerateThread()
        {
            MainForm.CheckForIllegalCrossThreadCalls = false;
            threadEntering = new Thread(new ThreadStart(EnterAVM));
            threadExit = new Thread(new ThreadStart(ExitAVM));
            threadControl = new Thread(new ThreadStart(controlThread));
            threadEle1 = new Thread(() => elevatorThread(0));
            threadEle2 = new Thread(() => elevatorThread(1));
            threadEle3 = new Thread(() => elevatorThread(2));
            threadEle4 = new Thread(() => elevatorThread(3));
            threadEle5 = new Thread(() => elevatorThread(4));
        }

        private void GenerateElevator()
        {
            eleList.Add(new Elevator { active = true, capacity = 10, countInside = 0, elevatorID = 1, floor = -1, mode = "working", target = 0, direction = "up" });
            eleList.Add(new Elevator { active = false, capacity = 10, countInside = 0, elevatorID = 2, floor = -1, mode = "idle", target = 0, direction = "up" });
            eleList.Add(new Elevator { active = false, capacity = 10, countInside = 0, elevatorID = 3, floor = -1, mode = "idle", target = 0, direction = "up" });
            eleList.Add(new Elevator { active = false, capacity = 10, countInside = 0, elevatorID = 4, floor = -1, mode = "idle", target = 0, direction = "up" });
            eleList.Add(new Elevator { active = false, capacity = 10, countInside = 0, elevatorID = 5, floor = -1, mode = "idle", target = 0, direction = "up" });
        }

        private void btnStartThreadClick(object sender, EventArgs e)
        {
            if (!threadEntering.IsAlive)
            {
                threadEntering.Start();
                threadExit.Start();
                threadControl.Start();
                threadEle1.Start();
                threadEle2.Start();
                threadEle3.Start();
                threadEle4.Start();
                threadEle5.Start();
            }
        }

        #endregion FormMethods

        #region ControlEnterExit

        private void controlThread()
        {
            while (true)
            {
                lock (peopleList)
                {
                    #region floorTexts

                    floor1.Text = peopleList.Where(p => p.currentFloor == 1).Count().ToString();
                    floor2.Text = peopleList.Where(p => p.currentFloor == 2).Count().ToString();
                    floor3.Text = peopleList.Where(p => p.currentFloor == 3).Count().ToString();
                    floor4.Text = peopleList.Where(p => p.currentFloor == 4).Count().ToString();
                    line0.Text = peopleList.Where(p => p.currentFloor == 0 && p.inLine == true).Count().ToString();
                    line1.Text = peopleList.Where(p => p.currentFloor == 1 && p.inLine == true).Count().ToString();
                    line2.Text = peopleList.Where(p => p.currentFloor == 2 && p.inLine == true).Count().ToString();
                    line3.Text = peopleList.Where(p => p.currentFloor == 3 && p.inLine == true).Count().ToString();
                    line4.Text = peopleList.Where(p => p.currentFloor == 4 && p.inLine == true).Count().ToString();

                    #endregion floorTexts

                    #region elevator1Texts

                    ele1f0.Text = eleList[0].insideList.Where(p => p.targetFloor == 0).Count().ToString();
                    ele1f1.Text = eleList[0].insideList.Where(p => p.targetFloor == 1).Count().ToString();
                    ele1f2.Text = eleList[0].insideList.Where(p => p.targetFloor == 2).Count().ToString();
                    ele1f3.Text = eleList[0].insideList.Where(p => p.targetFloor == 3).Count().ToString();
                    ele1f4.Text = eleList[0].insideList.Where(p => p.targetFloor == 4).Count().ToString();
                    var insieList0 = eleList[0].insideList.Select(p => p.personID).ToList();
                    ele1InsideList.Text = String.Join(",", insieList0);

                    #endregion elevator1Texts

                    #region elevator2Texts

                    ele2f0.Text = eleList[1].insideList.Where(p => p.targetFloor == 0).Count().ToString();
                    ele2f1.Text = eleList[1].insideList.Where(p => p.targetFloor == 1).Count().ToString();
                    ele2f2.Text = eleList[1].insideList.Where(p => p.targetFloor == 2).Count().ToString();
                    ele2f3.Text = eleList[1].insideList.Where(p => p.targetFloor == 3).Count().ToString();
                    ele2f4.Text = eleList[1].insideList.Where(p => p.targetFloor == 4).Count().ToString();
                    var insieList1 = eleList[1].insideList.Select(p => p.personID).ToList();
                    ele2InsideList.Text = String.Join(",", insieList1);

                    #endregion elevator2Texts

                    #region elevator3Texts

                    ele3f0.Text = eleList[2].insideList.Where(p => p.targetFloor == 0).Count().ToString();
                    ele3f1.Text = eleList[2].insideList.Where(p => p.targetFloor == 1).Count().ToString();
                    ele3f2.Text = eleList[2].insideList.Where(p => p.targetFloor == 2).Count().ToString();
                    ele3f3.Text = eleList[2].insideList.Where(p => p.targetFloor == 3).Count().ToString();
                    ele3f4.Text = eleList[2].insideList.Where(p => p.targetFloor == 4).Count().ToString();
                    var insieList2 = eleList[2].insideList.Select(p => p.personID).ToList();
                    ele3InsideList.Text = String.Join(",", insieList2);

                    #endregion elevator3Texts

                    #region elevator4Texts

                    ele4f0.Text = eleList[3].insideList.Where(p => p.targetFloor == 0).Count().ToString();
                    ele4f1.Text = eleList[3].insideList.Where(p => p.targetFloor == 1).Count().ToString();
                    ele4f2.Text = eleList[3].insideList.Where(p => p.targetFloor == 2).Count().ToString();
                    ele4f3.Text = eleList[3].insideList.Where(p => p.targetFloor == 3).Count().ToString();
                    ele4f4.Text = eleList[3].insideList.Where(p => p.targetFloor == 4).Count().ToString();
                    var insieList3 = eleList[3].insideList.Select(p => p.personID).ToList();
                    ele4InsideList.Text = String.Join(",", insieList3);

                    #endregion elevator4Texts

                    #region elevator5Texts

                    ele5f0.Text = eleList[4].insideList.Where(p => p.targetFloor == 0).Count().ToString();
                    ele5f1.Text = eleList[4].insideList.Where(p => p.targetFloor == 1).Count().ToString();
                    ele5f2.Text = eleList[4].insideList.Where(p => p.targetFloor == 2).Count().ToString();
                    ele5f3.Text = eleList[4].insideList.Where(p => p.targetFloor == 3).Count().ToString();
                    ele5f4.Text = eleList[4].insideList.Where(p => p.targetFloor == 4).Count().ToString();
                    var insieList4 = eleList[4].insideList.Select(p => p.personID).ToList();
                    ele5InsideList.Text = String.Join(",", insieList4);

                    #endregion elevator5Texts

                    #region ActiveControl

                    var lineCount = peopleList.Where(p => p.inLine == true).Count();
                    var activeEleList = eleList.Where(e => e.active == true).Count();

                    if (lineCount > activeEleList * 20)
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            if (eleList[i].active == false)
                            {
                                eleList[i].active = true;
                                ((Controls["grpEle" + i.ToString()] as GroupBox).Controls["ele" + i.ToString() + "Active"] as Label).Text = "Çalışıyor";
                                ((Controls["grpEle" + i.ToString()] as GroupBox).Controls["ele" + i.ToString() + "Active"] as Label).ForeColor = System.Drawing.Color.DarkGreen;
                                break;
                            }
                        }
                    }
                    else if (lineCount < activeEleList * 10)
                    {
                        if (activeEleList != 1)
                        {
                            if (eleList[activeEleList - 1].active == true) eleList[activeEleList - 1].active = false;
                            ((Controls["grpEle" + (activeEleList - 1).ToString()] as GroupBox).Controls["ele" + (activeEleList - 1).ToString() + "Active"] as Label).Text = "Beklemede";
                            ((Controls["grpEle" + (activeEleList - 1).ToString()] as GroupBox).Controls["ele" + (activeEleList - 1).ToString() + "Active"] as Label).ForeColor = System.Drawing.Color.Goldenrod;
                        }
                    }

                    #endregion ActiveControl
                }
                Thread.Sleep(10);
            }
        }

        private void EnterAVM()
        {
            while (true)
            {
                int peopleNumber = rnd.Next(1, 11);

                lock (peopleList)
                {
                    for (int i = 0; i < peopleNumber; i++)
                    {
                        peopleList.Add(new Person { personID = TotalPersonNumber, currentFloor = 0, targetFloor = rnd.Next(1, 5), inLine = true });
                        TotalPersonNumber++;
                    }
                }

                Thread.Sleep(500);
            }
        }

        private void ExitAVM()
        {
            while (true)
            {
                List<Person> lineList;
                int peopleNumber = rnd.Next(1, 6);
                lock (peopleList)
                {
                    lineList = peopleList.Where(p => p.inLine == false).ToList();
                    if (lineList.Count < peopleNumber) peopleNumber = lineList.Count();
                    if (lineList.Count > 0)
                    {
                        for (int i = 0; i < peopleNumber; i++)
                        {
                            int personId = lineList[rnd.Next(lineList.Count())].personID;
                            var person = peopleList.Where(p => p.personID == personId).FirstOrDefault();
                            person.targetFloor = 0;
                            person.inLine = true;
                        }
                    }
                }

                Thread.Sleep(1000);
            }
        }

        #endregion ControlEnterExit

        #region ElevatorRegion

        private void elevatorThread(int eleNum)
        {
            while (true)
            {
                if (eleList[eleNum].active == true || eleList[eleNum].countInside > 0)
                {
                    #region DirectionPart

                    if (eleList[eleNum].floor == 4) eleList[eleNum].direction = "down";
                    else if (eleList[eleNum].floor == 0) eleList[eleNum].direction = "up";

                    if (eleList[eleNum].direction == "up") eleList[eleNum].floor++;
                    else if (eleList[eleNum].direction == "down") eleList[eleNum].floor--;

                    (Controls["eve" + (eleNum + 1).ToString()] as GroupBox).Location = locationListEle[eleNum, eleList[eleNum].floor];

                    #endregion DirectionPart

                    #region ListOperations

                    List<Person> enteringElevatorList;
                    List<Person> leavingElevatorList = null;
                    lock (peopleList)
                    {
                        enteringElevatorList = peopleList.Where(p => p.currentFloor == eleList[eleNum].floor && p.inLine == true).ToList();
                        leavingElevatorList = eleList[eleNum].insideList.Where(p => p.targetFloor == eleList[eleNum].floor).ToList();
                    }

                    #endregion ListOperations

                    #region Leaving

                    if (leavingElevatorList.Count() != 0)
                    {
                        int loopEleCount = leavingElevatorList.Count();

                        for (int i = 0; i < loopEleCount; i++)
                        {
                            Person person;
                            lock (peopleList)
                            {
                                person = peopleList.Where(p => p.personID == leavingElevatorList[i].personID).FirstOrDefault();
                            }
                            if (person != null)
                            {
                                person.currentFloor = eleList[eleNum].floor;
                                person.targetFloor = 0;
                                person.inLine = false;
                                eleList[eleNum].insideList.Remove(leavingElevatorList[i]);
                                eleList[eleNum].countInside--;
                                ((Controls["eve" + (eleNum + 1).ToString()] as GroupBox).Controls["ele" + (eleNum + 1).ToString() + "Cap"] as TextBox).Text = eleList[eleNum].countInside + "/" + eleList[eleNum].capacity;
                            }
                        }
                    }

                    #endregion Leaving

                    #region Entering

                    if (eleList[eleNum].active == true)
                    {
                        int loopCount = eleList[eleNum].capacity - eleList[eleNum].countInside;
                        if (enteringElevatorList.Count() < loopCount) loopCount = enteringElevatorList.Count();

                        for (int i = 0; i < loopCount; i++)
                        {
                            Person person;
                            lock (peopleList)
                            {
                                person = peopleList.Where(p => p.personID == enteringElevatorList[i].personID).FirstOrDefault();
                                if (person != null)
                                {
                                    eleList[eleNum].insideList.Add(person);
                                    eleList[eleNum].countInside++;                               
                                    ((Controls["eve" + (eleNum + 1).ToString()] as GroupBox).Controls["ele" + (eleNum + 1).ToString() + "Cap"] as TextBox).Text = eleList[eleNum].countInside + "/" + eleList[eleNum].capacity;
                                    person.currentFloor = -1;
                                }
                            }
                          
                        }
                    }

                    #endregion Entering
                }
                Thread.Sleep(600);
            }
        }

        #endregion ElevatorRegion
    }
}