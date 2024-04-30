using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Data.SQLite;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace SeleniumTEST
{
    public partial class Form1 : Form
    {
        int companycounter = 0;
        int sayimiz = 0;
        IWebDriver driver;

        public Form1()
        {
            InitializeComponent();
        
        }
        public void Form1_Load(object sender, EventArgs e)
        {
             SQLiteConnection go2 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
             SQLiteDataAdapter adapter2 = new SQLiteDataAdapter("select * from DataCompany", go2);
             DataSet datasetim2 = new DataSet();
             adapter2.Fill(datasetim2, "DataCompany");
             dataGridView2.DataSource = datasetim2.Tables[0];
             go2.Dispose(); datasetim2.Dispose(); adapter2.Dispose();     
                               
        }
       
        public void Baslat()
        {

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            Control.CheckForIllegalCrossThreadCalls = false;
            driver = new ChromeDriver(chromeDriverService);
            driver.Manage().Window.Size = new Size(800, 800);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        enbas:

            int artir = 2;   
            driver.Navigate().GoToUrl("https://www.google.com/maps/");
          wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"searchboxinput\"]")));
            IWebElement elementim = driver.FindElement(By.XPath("//*[@id=\"searchboxinput\"]"));
            elementim.SendKeys(search_box_text.Text);
            IWebElement searchButton = driver.FindElement(By.Id("searchbox-searchbutton"));
            searchButton.Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"QA0Szd\"]/div/div/div[1]/div[2]/div/div[1]/div/div/div[1]/div[1]/div[3]/div")));
            IWebElement ilkParagraf = driver.FindElement(By.XPath("//*[@id=\"QA0Szd\"]/div/div/div[1]/div[2]/div/div[1]/div/div/div[1]/div[1]/div[3]/div"));
            ilkParagraf.Click();

        // SEARCHED RESULTS CODE HERE. IT WILL OPEN THEM ONE BY ONE IN A NEW TAB AND EXTRACT INFORMATION
            artir = 2;
          TumVeriler:
            // SQLITE CONNEXION
            string conn = @"Data Source=company_data.db;Version=3;New=False";
                string conn2 = @"Data Source=gecici.db;Version=3;";
            SQLiteConnection sqliteCon = new SQLiteConnection(conn);
                 SQLiteConnection sqliteCon2 = new SQLiteConnection(conn2);
            sqliteCon.Open();
               sqliteCon2.Open();
               
            SQLiteDataAdapter adapter = new SQLiteDataAdapter("Select * from DataCompany", sqliteCon);
               SQLiteDataAdapter adapter2 = new SQLiteDataAdapter("select * from DataCompany", sqliteCon2);
               
            SQLiteCommand AddCommand = new SQLiteCommand("insert into DataCompany (Date, CompanyName, SearchKeyword, State, Country, Stars, Comment, Category, Adress, WorkingHours, Website, PhoneNumber, PlusCode) values (@Date, @CompanyName, @SearchKeyword, @State, @Country, @Stars, @Comment, @Category, @Adress, @WorkingHours, @Website, @PhoneNumber, @PlusCode)", sqliteCon);
               SQLiteCommand AddCommand2 = new SQLiteCommand("insert into DataCompany (Date, CompanyName, SearchKeyword, State, Country, Stars, Comment, Category, Adress, WorkingHours, Website, PhoneNumber, PlusCode) values (@Date, @CompanyName, @SearchKeyword, @State, @Country, @Stars, @Comment, @Category, @Adress, @WorkingHours, @Website, @PhoneNumber, @PlusCode)", sqliteCon2);
               
            string tarih = DateTime.Now.ToString();
            AddCommand.Parameters.AddWithValue("@Date", tarih);
                AddCommand2.Parameters.AddWithValue("@Date", tarih);
                
            AddCommand.Parameters.AddWithValue("@SearchKeyword", search_box_text.Text);
               AddCommand2.Parameters.AddWithValue("@SearchKeyword", search_box_text.Text);

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("#QA0Szd > div > div > div.w6VYqd > div.bJzME.Hu9e2e.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.m6QErb.DxyBCb.kA9KIf.dS8AEf > div.TIHn2 > div > div.lMbq3e > div:nth-child(1) > h1 > span.a5H0ec")));
                IWebElement companyName = driver.FindElement(By.CssSelector("#QA0Szd > div > div > div.w6VYqd > div.bJzME.Hu9e2e.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.m6QErb.DxyBCb.kA9KIf.dS8AEf > div.TIHn2 > div > div.lMbq3e > div:nth-child(1) > h1 > span.a5H0ec"));
                
                AddCommand.Parameters.AddWithValue("@CompanyName", companyName.Text);
                    AddCommand2.Parameters.AddWithValue("@CompanyName", companyName.Text);
              
                companycounter += 1;
                label3.Text = companycounter.ToString();

            }
            catch (Exception ex)
           
            {
                AddCommand.Parameters.AddWithValue("@CompanyName", "ŞİRKET ADI YOK");
                  AddCommand2.Parameters.AddWithValue("@CompanyName", "ŞİRKET ADI YOK");
                Console.WriteLine("Failed to find Company Name: " + ex.Message);

            }
            try
            {

                IWebElement adres = driver.FindElement(By.ClassName("AeaXub"));
                AddCommand.Parameters.AddWithValue("@Adress", adres.Text);
                  AddCommand2.Parameters.AddWithValue("@Adress", adres.Text);
                

            }
            catch (Exception ex)
            {
                AddCommand.Parameters.AddWithValue("@Adress", "Adres bulunamadı");
                   AddCommand2.Parameters.AddWithValue("@Adress", "Adres bulunamadı");
                Console.WriteLine("Failed to find Address Name: " + ex.Message);

            }
            try
            {

                IWebElement hours = driver.FindElement(By.ClassName("G8aQO"));
                AddCommand.Parameters.AddWithValue("@WorkingHours", hours.Text);
                    AddCommand2.Parameters.AddWithValue("@WorkingHours", hours.Text);

            }
            catch (Exception ex)
            {
                AddCommand.Parameters.AddWithValue("@WorkingHours", "YOK");
                    AddCommand2.Parameters.AddWithValue("@WorkingHours", "YOK");
                Console.WriteLine("Failed to find Working Hours: " + ex.Message);
            }
            try
            {
                IWebElement website = driver.FindElement(By.CssSelector("[data-item-id = 'authority']"));
                AddCommand.Parameters.AddWithValue("@Website", website.Text);
                    AddCommand2.Parameters.AddWithValue("@Website", website.Text);

            }
            catch (Exception ex)
            {
                AddCommand.Parameters.AddWithValue("@Website", "WEB SİTESİ YOK");
                  AddCommand2.Parameters.AddWithValue("@Website", "WEB SİTESİ YOK");
                Console.WriteLine("Failed to find Website: " + ex.Message);

            }
            try
            {

                IWebElement phone = driver.FindElement(By.CssSelector("#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(9) > div:nth-child(6) > button > div > div.rogA2c > div.Io6YTe.fontBodyMedium.kR99db"));
                AddCommand.Parameters.AddWithValue("@PhoneNumber", phone.Text);
                    AddCommand2.Parameters.AddWithValue("@PhoneNumber", phone.Text);

            }
            catch (Exception ex)
            {
                AddCommand.Parameters.AddWithValue("@PhoneNumber", "Telefon YOK");
                 AddCommand2.Parameters.AddWithValue("@PhoneNumber", "Telefon YOK");   
                Console.WriteLine("Failed to find Phone Number: " + ex.Message);
                    
                    

            }
            try
            {
                IWebElement plusCode = driver.FindElement(By.CssSelector("[data-item-id= 'oloc']"));
                AddCommand.Parameters.AddWithValue("@PlusCode", plusCode.Text);
                  AddCommand2.Parameters.AddWithValue("@PlusCode", plusCode.Text);

                string ayir = plusCode.Text;

                string KalanYazi = ayir.Remove(0, 8);

                string[] parcala;

                parcala = KalanYazi.Split(',');

                int son = parcala.Length - 2;
                int son1 = parcala.Length - 1;

                string yaziyim = parcala[son];
                string yaziyim2 = parcala[son1];

                AddCommand.Parameters.AddWithValue("@State", yaziyim.TrimStart());
                   AddCommand2.Parameters.AddWithValue("@State", yaziyim.TrimStart());

                AddCommand.Parameters.AddWithValue("@Country", yaziyim2.TrimStart());
                  AddCommand2.Parameters.AddWithValue("@Country", yaziyim2.TrimStart());

            }
            catch (Exception ex)
            {
                AddCommand.Parameters.AddWithValue("@PlusCode", "Plus Code Yok");
                    AddCommand2.Parameters.AddWithValue("@PlusCode", "Plus Code Yok");
                Console.WriteLine("Failed to find Plus Code: " + ex.Message);

            }
            try
            {

                IWebElement comments = driver.FindElement(By.CssSelector("#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div > div.lMbq3e > div.LBgpqf > div > div.fontBodyMedium.dmRWX > div.F7nice > span:nth-child(2) > span > span"));
                AddCommand.Parameters.AddWithValue("@Comment", comments.Text);
                AddCommand2.Parameters.AddWithValue("@Comment", comments.Text);

            }
            catch (Exception ex)
            {
                AddCommand.Parameters.AddWithValue("@Comment", "Bulunamadı");
                AddCommand2.Parameters.AddWithValue("@Comment", "Bulunamadı");
                Console.WriteLine("Failed to find Comment: " + ex.Message);

            }
            try
            {

                IWebElement stars = driver.FindElement(By.CssSelector("#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div > div.lMbq3e > div.LBgpqf > div > div.fontBodyMedium.dmRWX > div.F7nice > span:nth-child(1) > span:nth-child(1)"));
                AddCommand.Parameters.AddWithValue("@Stars", stars.Text);
                AddCommand2.Parameters.AddWithValue("@Stars", stars.Text);

            }
            catch (Exception ex)
            {

                AddCommand.Parameters.AddWithValue("@Stars", "No Stars");
                AddCommand2.Parameters.AddWithValue("@Stars", "No Stars");
                    Console.WriteLine("Failed to find Stars: " + ex.Message);

            }
            try
            {

                IWebElement category = driver.FindElement(By.CssSelector("#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div > div.lMbq3e > div.LBgpqf > div > div:nth-child(2) > span > span > button"));
                AddCommand.Parameters.AddWithValue("@Category", category.Text);            
                AddCommand2.Parameters.AddWithValue("@Category", category.Text);

            }
            catch (Exception ex)
            {
                AddCommand.Parameters.AddWithValue("@Category", "Category not found");
                AddCommand2.Parameters.AddWithValue("@Category", "Category not found");
                Console.WriteLine("Failed to find Category: " + ex.Message);

            }
            try
            {

                AddCommand.ExecuteNonQuery();
                AddCommand2.ExecuteNonQuery();
                sqliteCon.Close(); sqliteCon2.Close();
                sqliteCon.Dispose(); sqliteCon2.Dispose();

            }
            catch (Exception)
            {
               
            }
            try
            {
                // SWITCH TO NEXT COMPANY
              
                IWebElement childOk = driver.FindElement(By.CssSelector("#sGi9mc-m5SR9c-bottom-pane > div > div.Yr7JMd-pane-content.cYB2Ge-oHo7ed > div > div > div > div.xtu1r-K9a4Re-XuHpsb-haAclf > div.xtu1r-K9a4Re-ibnC6b-haAclf > div:nth-child(" + artir + ")"));
                                                                          
                artir += 1;        
                childOk.Click();
                Thread.Sleep(2000);  
                goto TumVeriler;

            }
            catch (NoSuchElementException)
            {

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("xoLGzf")));
                IWebElement backButton = driver.FindElement(By.ClassName("xoLGzf"));
                searchButton.Click();      
                
                try
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"QA0Szd\"]/div/div/div[1]/div[2]/div/div[1]/div/div/div[1]/div[1]/div[5]/div")));
                    IWebElement nextPage = driver.FindElement(By.XPath("//*[@id=\"QA0Szd\"]/div/div/div[1]/div[2]/div/div[1]/div/div/div[1]/div[1]/div[5]/div"));
                    nextPage.Click();
                }
                catch (Exception)
                {
                    driver.Quit();
                }
                try
                {
                    Thread.Sleep(4000);   
                    driver.FindElement(By.CssSelector("#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.m6QErb.DxyBCb.kA9KIf.dS8AEf.ecceSd > div.m6QErb.DxyBCb.kA9KIf.dS8AEf.ecceSd > div:nth-child(3) > div")).Click(); // First Company
                    artir = 2;
                    goto TumVeriler;

                }
                catch (Exception)
                {
                    label4.Text = "Process Completed";
                    label4.Refresh();
                    MessageBox.Show("Process Completed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    driver.Quit();
                }
            }
        }       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void search_box_text_TextChanged(object sender, EventArgs e)
        {

        }
        private void UpdateData_Click(object sender, EventArgs e)
        {

            SQLiteConnection go4 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
            SQLiteDataAdapter adapter4 = new SQLiteDataAdapter("select * from DataCompany", go4);
            DataSet datasetim4 = new DataSet();
            adapter4.Fill(datasetim4, "DataCompany");
            dataGridView2.DataSource = datasetim4.Tables[0];
            go4.Dispose(); datasetim4.Dispose(); adapter4.Dispose();

        }  
        private void label3_Click(object sender, EventArgs e)
        {
        
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void UpdataDataButton_Click(object sender, EventArgs e)
        {

            SQLiteConnection go4 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
            SQLiteDataAdapter adapter4 = new SQLiteDataAdapter("select * from DataCompany", go4);
            DataSet datasetim4 = new DataSet();
            adapter4.Fill(datasetim4, "DataCompany");
            dataGridView2.DataSource = datasetim4.Tables[0];
            go4.Dispose(); datasetim4.Dispose(); adapter4.Dispose();

        }

        private void export_Button_Click(object sender, EventArgs e)
        {

            SQLiteConnection go4 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
            SQLiteDataAdapter adapter4 = new SQLiteDataAdapter("select * from DataCompany", go4);
            DataSet datasetim4 = new DataSet();
            adapter4.Fill(datasetim4, "DataCompany");
            dataGridView2.DataSource = datasetim4.Tables[0];
            go4.Dispose(); datasetim4.Dispose(); adapter4.Dispose();

            if (dataGridView2.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk.", MessageBoxIcon.Asterisk + ex.Message, MessageBoxButtons.OK);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView2.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView2.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView2.Columns[i].HeaderText.ToString() + "*";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView2.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView2.Rows[i - 1].Cells[j].Value?.ToString() + "*";
                                }
                            }
                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);

                            MessageBox.Show("Data Exported Successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :", MessageBoxIcon.Error + ex.Message, MessageBoxButtons.OK);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowHideButton_Click(object sender, EventArgs e)
        {
            if (driver == null)
            {
                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                Control.CheckForIllegalCrossThreadCalls = false;
                driver = new ChromeDriver(chromeDriverService);
                driver.Manage().Window.Size = new Size(800, 800);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            }

            if (sayimiz == 0)
            {
                driver.Manage().Window.Minimize();
                sayimiz = 1;
            }
            else
            {
                driver.Manage().Window.Size = new Size(400, 400);
                sayimiz = 0;
            }
        }
        private void ClearDataButton_Click(object sender, EventArgs e)
        {
            SQLiteConnection go3 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
            SQLiteDataAdapter adapter3 = new SQLiteDataAdapter("delete from DataCompany", go3);
            DataSet datasetim3 = new DataSet();
            adapter3.Fill(datasetim3, "DataCompany");
            dataGridView2.DataSource = datasetim3;
            go3.Dispose(); datasetim3.Dispose(); adapter3.Dispose();
        
            SQLiteConnection go5 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
            SQLiteDataAdapter adapter5 = new SQLiteDataAdapter("select * from DataCompany", go5);
            DataSet datasetim5 = new DataSet();
            adapter5.Fill(datasetim5, "DataCompany");
            dataGridView2.DataSource = datasetim5.Tables[0];
            go5.Dispose(); datasetim5.Dispose(); adapter5.Dispose();
            
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            int sayimiz = 0;
            int companycounter = 0;

            label3.Text = companycounter.ToString();
            label3.Refresh();

            Thread islem = new Thread(new ThreadStart(Baslat));
            islem.Start();
            label4.Text = " ";

            SQLiteConnection go3 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
            SQLiteDataAdapter adapter3 = new SQLiteDataAdapter("delete from DataCompany", go3);
            DataSet datasetim3 = new DataSet();
            adapter3.Fill(datasetim3, "DataCompany");
            dataGridView2.DataSource = datasetim3;
            go3.Dispose(); datasetim3.Dispose(); adapter3.Dispose();

            SQLiteConnection go5 = new SQLiteConnection(@"Data source=gecici.db;Version=3;New=False");
            SQLiteDataAdapter adapter5 = new SQLiteDataAdapter("select * from DataCompany", go5);
            DataSet datasetim5 = new DataSet();
            adapter5.Fill(datasetim5, "DataCompany");
            dataGridView2.DataSource = datasetim5.Tables[0];
            go5.Dispose(); datasetim5.Dispose(); adapter5.Dispose();

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
