namespace Final;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
class DataTier
{
    public string connStr = "server=20.172.0.16;user=xwang8;database=xwang8;port=8080;password=xwang8";

    // perform login check using Stored Procedure "LoginCount" in Database based on given user' studentID and Password
    public bool LoginCheck(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "StaffLoginCount";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure; // set the commandType as storedProcedure
            cmd.Parameters.AddWithValue("@inputUserName", user.userName);
            cmd.Parameters.AddWithValue("@inputUserPassword", user.userPassword);
            cmd.Parameters.Add("@staffCount", MySqlDbType.Int32).Direction = ParameterDirection.Output;
            MySqlDataReader rdr = cmd.ExecuteReader();

            int returnCount = (int)cmd.Parameters["@staffCount"].Value;
            rdr.Close();
            conn.Close();

            if (returnCount == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return false;
        }

    }
    // 
    public bool PackageCheck(string full_name, int unit_number)
    {
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();
            string procedure = "MatchResidents";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure; // set the commandType as storedProcedure
            cmd.Parameters.AddWithValue("@inputResidentName", full_name);
            cmd.Parameters.AddWithValue("@inputUnitNumber", unit_number);
            cmd.Parameters.Add("@residentCount", MySqlDbType.Int32).Direction = ParameterDirection.Output;
            MySqlDataReader rdr = cmd.ExecuteReader();

            int returnCount = (int)cmd.Parameters["@residentCount"].Value;
            rdr.Close();
            conn.Close();

            if (returnCount == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return false;
        }

    }
    // Add Packages
    public void AddToPendingArea(int unit_number, string full_name, string post_agency)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "AddToPendingArea";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputUnitNumber", unit_number);
            cmd.Parameters["@inputUnitNumber"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputResidentName", full_name);
            cmd.Parameters["@inputResidentName"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputPostServiceAgency", post_agency);
            cmd.Parameters["@inputPostServiceAgency"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
        }

    }

    public DataTable ShowAllPending(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();
            string procedure = "ShowAllPending";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;


            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableAllPending = new DataTable();
            tableAllPending.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableAllPending;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }
    }
    public void AddToHistory(int unit_number, string full_name, string post_agency)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "AddToHistory";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputUnitNumber", unit_number);
            cmd.Parameters["@inputUnitNumber"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputResidentName", full_name);
            cmd.Parameters["@inputResidentName"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputPostServiceAgency", post_agency);
            cmd.Parameters["@inputPostServiceAgency"].Direction = ParameterDirection.Input;


            MySqlDataReader rdr = cmd.ExecuteReader();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
        }

    }

    public DataTable ShowHistory(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        Console.WriteLine("Please input the unit numbrt");
        int unit_number = Convert.ToInt16(Console.ReadLine());
        Console.WriteLine("Please input the resident name");
        string full_name = Console.ReadLine();


        try
        {
            conn.Open();
            string procedure = "ShowHistory";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputUnitNumber", unit_number);
            cmd.Parameters["@inputUnitNumber"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputResidentName", full_name);
            cmd.Parameters["@inputResidentName"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableRecords = new DataTable();
            tableRecords.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableRecords;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }



    }


    public DataTable ShowPending(User user, int unit_number, string full_name)
    {
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();
            string procedure = "ShowPending";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputUnitNumber", unit_number);
            cmd.Parameters["@inputUnitNumber"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputResidentName", full_name);
            cmd.Parameters["@inputResidentName"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableRecords = new DataTable();
            tableRecords.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableRecords;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }

    }

    public void DeleteFromPending(int unit_number, string full_name)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "DeleteFromPending";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputUnitNumber", unit_number);
            cmd.Parameters["@inputUnitNumber"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputResidentName", full_name);
            cmd.Parameters["@inputResidentName"].Direction = ParameterDirection.Input;


            MySqlDataReader rdr = cmd.ExecuteReader();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
        }

    }

    // problem here, void cannot be convert to string
    public string ResidentEmail(int unit_number, string full_name)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        
        try
        {
            conn.Open();
            string procedure = "ResidentEmail";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputUnitNumber", unit_number);
            cmd.Parameters["@inputUnitNumber"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputResidentName", full_name);
            cmd.Parameters["@inputResidentName"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();
            string email = string.Empty;
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    email = rdr.GetString(0);
                    break;
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            rdr.Close();

            return email;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }

    }


// Add package to Unknow Area
 public DataTable AddToUnknow(string packageOwner, string postServiceAgency, string deliveryDate)
    {
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();
            string procedure = "AddToUnknowArea";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputPackageOwner", packageOwner);
            cmd.Parameters["@inputPackageOwner"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputPostServiceAgency", postServiceAgency);
            cmd.Parameters["@inputPostServiceAgency"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputDeliveryDate", deliveryDate);
            cmd.Parameters["@inputDeliveryDate"].Direction = ParameterDirection.Input;

            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableRecords = new DataTable();
            tableRecords.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableRecords;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }

    }

    public DataTable ShowUnknow(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();
            string procedure = "ShowUnknow";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            

            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableRecords = new DataTable();
            tableRecords.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableRecords;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }

    }

    public void DeleteUnknow(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();
            string procedure = "DeleteFromUnknow";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            

            MySqlDataReader rdr = cmd.ExecuteReader();

        
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
          
        }

    }
}