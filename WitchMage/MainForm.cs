using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Timers;
using System.Windows.Threading;

namespace WitchMage
{
    public partial class MainForm : Form
    {


        private int currentStepID = 0;
        private Step currentStep;

        StepSequence stepSequence = new StepSequence();
        Brand newBrand = new Brand();
        List<Step> newSteps;
        List<StepComponent> stepInputs;
        List<Evaluation> stepEvals;
        List<Action> stepActions;


        List<TextBox> currentlyLoadedTextBoxes = new List<TextBox>();
        List<RichTextBox> currentlyLoadedRichTextBoxes = new List<RichTextBox>();
        List<Button> currentlyLoadedButtons = new List<Button>();
        List<PictureBox> currentlyLoadedPictureBoxes = new List<PictureBox>();
        List<Label> currentlyLoadedLabels = new List<Label>();
        List<CheckBox> currentlyLoadedCheckBoxes = new List<CheckBox>();
        List<ProgressBar> currentlyLoadedProgressBar = new List<ProgressBar>();

        private static System.Timers.Timer evalTimer;
          

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //First Load should load files and load first Form

            LoadFiles();

            currentStep = newSteps[currentStepID];

            ApplyBranding(newBrand);
            ProcessStepInputs(stepInputs);
            ProcessEvals(stepEvals);

            SetTimer();
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            evalTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer.
            evalTimer.Elapsed += OnTimedEvent;
            evalTimer.AutoReset = true;
            evalTimer.Enabled = true;

        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ProcessEvals(stepEvals);
        }

        public List<Evaluation> GetCurrentEvaluations()
        {
            return stepEvals;
        }

        private void AdvanceStep()
        {
            currentStepID++;
            ReloadForm();
        }
        private void PreviousStep()
        {
            currentStepID--;
            ReloadForm();
        }

        private void ReloadForm()
        {
            ClearForm();
            currentStep = newSteps[currentStepID];
            ApplyBranding(newBrand);
            ProcessStepInputs(stepInputs);
            ProcessEvals(stepEvals);

        }

        private void ClearForm()
        {

            foreach (TextBox txtbox in currentlyLoadedTextBoxes)
            {
                this.Controls.Remove(txtbox);
            }
            foreach (RichTextBox rtxtbox in currentlyLoadedRichTextBoxes)
            {
                this.Controls.Remove(rtxtbox);
            }
            foreach (Button btn in currentlyLoadedButtons)
            {
                this.Controls.Remove(btn);
            }
            foreach (PictureBox picture in currentlyLoadedPictureBoxes)
            {
                this.Controls.Remove(picture);
            }
            foreach (Label lbl in currentlyLoadedLabels)
            {
                this.Controls.Remove(lbl);
            }
            foreach (CheckBox chkBox in currentlyLoadedCheckBoxes)
            {
                this.Controls.Remove(chkBox);
            }
            foreach (ProgressBar progressBar in currentlyLoadedProgressBar)
            {
                this.Controls.Remove(progressBar);
            }
        }


        private void runStepAction(int actionID)
        {

            foreach (Action actionToExecute in stepActions)
            {
                if(actionToExecute.id == actionID)
                {
                    if(actionToExecute.type == "openURL")
                    {
                        Process.Start(actionToExecute.actionParam1);
                    }
                    if (actionToExecute.type == "runExecutable")
                    {

                        FileSystemOperations.ExecuteProcess(actionToExecute.actionParam1, actionToExecute.actionParam2, actionToExecute.runAsAdmin);
                    }

                    if (actionToExecute.type == "nextStep")
                    {
                        AdvanceStep();
                    }
                   
                    if (actionToExecute.type == "previousStep")
                    {
                        PreviousStep();
                    }

     
                    if (actionToExecute.type == "enableFormElement")
                    {
                        Control[] controlsByName = this.Controls.Find(actionToExecute.formItemNameToEnable, true);

                        foreach (Control con in controlsByName)
                        {


                            con.Invoke(new MethodInvoker(delegate
                            {
                                con.Enabled = true;
                                con.Visible = true;
                                con.Refresh();
                            }
                        ));

                        }
                    }

                    if (actionToExecute.type == "disableFormElement")
                    {
                        Control[] controlsByName = this.Controls.Find(actionToExecute.formItemNameToEnable, true);

                        foreach (Control con in controlsByName)
                        {


                            con.Invoke(new MethodInvoker(delegate
                            {
                                con.Enabled = false;
                                con.Visible = false;
                                con.Refresh();
                            }
                        ));

                        }
                    }

                    if (actionToExecute.type == "quit")
                    {
                        Application.Exit();
                    }


                }
                    
            }
        }


        private void genericButton_Click(object sender, System.EventArgs e)
        {
        
            foreach (StepComponent si in stepInputs)
            {
                if (si.stepID == currentStepID)
                {
                    Button senderBut = (Button)sender;
                    if(senderBut!=null)
                    {
                        if (senderBut.Name == si.id) //todo  lookup no need to for each
                        {

                            foreach (Action actionToExecute in stepActions)
                            {
                                if (actionToExecute.id == si.actionID)
                                {
                                    runStepAction(actionToExecute.id);
                                }


                            }

                        }
                    }
                    

                    
                }

            }

        }

        private void LoadFiles()
        {

            stepSequence = FileSystemOperations.LoadConfigFromFile();
            newBrand = stepSequence.stepBranding;
            newSteps = stepSequence.steps;
            stepInputs = stepSequence.stepInputs;
            stepEvals = stepSequence.stepEvaluations;
            stepActions = stepSequence.stepActions;
        }

        private void ProcessStepInputs(List<StepComponent> stepsToProcess)
        {
            //Process Step
            foreach (StepComponent stepinput in stepsToProcess)
            {
                if (stepinput.stepID == currentStep.id)
                {


                    if (stepinput.type == "TextBox")
                    {
                        Debug.WriteLine("Send to debug output.");
                        TextBox newtxtBox = new TextBox();
                        newtxtBox.Visible = stepinput.isVisible;
                        newtxtBox.Size = new Size(stepinput.sizeWidth, stepinput.sizeHeight);
                        newtxtBox.Location = new Point(stepinput.locationX, stepinput.locationY);
                        newtxtBox.Text = stepinput.text;
                        newtxtBox.ReadOnly = stepinput.isReadOnly;
                        newtxtBox.Height = stepinput.sizeHeight;
                        newtxtBox.Width = stepinput.sizeWidth;
                        newtxtBox.Font = new Font(newBrand.FontName, stepinput.fontSize, FontStyle.Regular);

                        this.Controls.Add(newtxtBox);
                        currentlyLoadedTextBoxes.Add(newtxtBox);

                    }

                    if (stepinput.type == "RichTextBox")
                    {
                        Debug.WriteLine("Send to debug output.");
                        RichTextBox newrichtxtBox = new RichTextBox();
                        newrichtxtBox.Visible = stepinput.isVisible;
                        newrichtxtBox.Size = new Size(stepinput.sizeWidth, stepinput.sizeHeight);
                        newrichtxtBox.Location = new Point(stepinput.locationX, stepinput.locationY);
                        newrichtxtBox.Text = stepinput.text;
                        newrichtxtBox.ReadOnly = stepinput.isReadOnly;
                        newrichtxtBox.Height = stepinput.sizeHeight;
                        newrichtxtBox.Width = stepinput.sizeWidth;
                        newrichtxtBox.BackColor = Util.ColorFromHex(newBrand.BackgroundColorHex);
                        if (stepinput.hasBorder == false)
                        {
                            newrichtxtBox.BorderStyle = BorderStyle.None;
                        }
                        newrichtxtBox.AcceptsTab = false;
                        newrichtxtBox.SelectionStart = 0;
                        newrichtxtBox.DetectUrls = true;
                        newrichtxtBox.Font = new Font(newBrand.FontName, stepinput.fontSize, FontStyle.Regular);
                        newrichtxtBox.ForeColor = Util.ColorFromHex(newBrand.TextColorHex);

                        this.Controls.Add(newrichtxtBox);
                        currentlyLoadedRichTextBoxes.Add(newrichtxtBox);

                    }


                    if (stepinput.type == "Label")
                    {

                        Label newLabel = new Label();
                        newLabel.Visible = stepinput.isVisible;
                        newLabel.Size = new Size(stepinput.sizeWidth, stepinput.sizeHeight);
                        newLabel.Location = new Point(stepinput.locationX, stepinput.locationY);
                        newLabel.Text = stepinput.text;
                        newLabel.Font = new Font(newBrand.FontName, stepinput.fontSize, FontStyle.Bold);
                        newLabel.ForeColor = Util.ColorFromHex(newBrand.TextColorHex);

                        this.Controls.Add(newLabel);
                        currentlyLoadedLabels.Add(newLabel);


                    }

                    if (stepinput.type == "Button")
                    {

                        Button newButton = new Button();
                        newButton.Visible = stepinput.isVisible;
                        newButton.Size = new Size(stepinput.sizeWidth, stepinput.sizeHeight);
                        newButton.Location = new Point(stepinput.locationX, stepinput.locationY);
                        newButton.Text = stepinput.text;
                        newButton.ForeColor = Util.ColorFromHex(newBrand.ActionTextColorHex);
                        newButton.BackColor = Util.ColorFromHex(newBrand.ActionButtonColorHex);
                        newButton.Font = new Font(newBrand.FontName, stepinput.fontSize, FontStyle.Bold);
                        newButton.Name = stepinput.id;

                        
                        newButton.Click += genericButton_Click;
                        


                        this.Controls.Add(newButton);
                        currentlyLoadedButtons.Add(newButton);

                        if (stepinput.bringToFront == true)
                        {
                            newButton.BringToFront();
                        }


                    }


                    if (stepinput.type == "Image")
                    {

                        PictureBox newImage = new PictureBox();
                        newImage.Visible = stepinput.isVisible;
                        newImage.ClientSize = new Size(stepinput.sizeWidth, stepinput.sizeHeight);
                        newImage.Location = new Point(stepinput.locationX, stepinput.locationY);
                        newImage.Name = stepinput.id;
                        Bitmap newImageBitMap = new Bitmap(stepinput.imagePath);
                        newImage.SizeMode = PictureBoxSizeMode.StretchImage;
                        newImage.Image = (Image)newImageBitMap;
                        newImage.Enabled = stepinput.isEnabled;
                        this.Controls.Add(newImage);
                        currentlyLoadedPictureBoxes.Add(newImage);
                    }


                    if (stepinput.type == "CheckBox")
                    {
                        CheckBox newCheckBox = new CheckBox();
                        newCheckBox.Checked = stepinput.isChecked;
                        newCheckBox.Enabled = stepinput.isEnabled;
                        newCheckBox.Visible = stepinput.isVisible;
                        newCheckBox.Height = stepinput.sizeHeight;
                        newCheckBox.Width = stepinput.sizeWidth;
                        newCheckBox.Location = new Point(stepinput.locationX, stepinput.locationY);
                        this.Controls.Add(newCheckBox);
                        currentlyLoadedCheckBoxes.Add(newCheckBox);
                    }


                    if (stepinput.type == "ProgressBox")
                    {
                        ProgressBar newProgressBar = new ProgressBar();
                        newProgressBar.Enabled = stepinput.isEnabled;
                        newProgressBar.Visible = stepinput.isVisible;
                        newProgressBar.Height = stepinput.sizeHeight;
                        newProgressBar.Width = stepinput.sizeWidth;
                        newProgressBar.Value = 90;

                        newProgressBar.Location = new Point(stepinput.locationX, stepinput.locationY);
                        this.Controls.Add(newProgressBar);
                        currentlyLoadedProgressBar.Add(newProgressBar);
                    }

                }


            }
        }

        private void ProcessEvals(List<Evaluation> evalsToProcess)
        {
         
            foreach (Evaluation eval in evalsToProcess)
            {
                int actionIDToExecute = -1;

                if (eval.validForStep == currentStepID )
                {
                    
                  
                    if (eval.type == "regcheck")
                    {
                        string evaledValue = (string)Registry.GetValue(eval.path, eval.checkKey, null);

                        if(evaledValue == eval.checkValue)
                        {
                            actionIDToExecute = eval.evaluationActionID;
                        }

                    }

                    if (eval.type == "processrunning")
                    {
                        Process[] pname = Process.GetProcessesByName(eval.checkValue);
                        if(pname.Length > 0)
                        {
                            actionIDToExecute = eval.evaluationActionID;
                        }

                    }


                    if (actionIDToExecute != -1)
                    {
                        runStepAction(eval.evaluationActionID);
                    }
                    
                }

            }
        }

        private void EnableControl(Control conToEnable)
        {
            conToEnable.Enabled = true;
            conToEnable.Visible = true;
            conToEnable.Refresh();
        }

        private void ApplyBranding(Brand brandToProcess)
        {
            //Form General (Branding)
            BackColor = Util.ColorFromHex(brandToProcess.BackgroundColorHex);
            Bitmap newLogo = new Bitmap(SetupFiles.SettingsRelativePath + "/" + brandToProcess.LogoImage);
            picture_Logo.Image = newLogo;
            this.Text = brandToProcess.FormName;

        }







    }
}
