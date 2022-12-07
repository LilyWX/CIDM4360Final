using Final;
using System.Data;
using MySql.Data.MySqlClient;
class GuiTier
{
    User user = new User();
    DataTier database = new DataTier();

    // print login page
    public User Login()
    {
        Console.WriteLine("------Welcome to Package Management Software------");
        Console.WriteLine("Please input user name: ");
        user.userName = Console.ReadLine();
        Console.WriteLine("Please input password: ");
        user.userPassword = Console.ReadLine();
        return user;
    }
    // print Dashboard after user logs in successfully
    public int Dashboard(User user)
    {
        DateTime localDate = DateTime.Now;
        Console.WriteLine("---------------Dashboard-------------------");
        Console.WriteLine($"Hello: {user.userName}; Date/Time: {localDate.ToString()}");
        Console.WriteLine("Please select an option to continue:");
        Console.WriteLine("1. Add A Package");
        Console.WriteLine("2. Pick A Package");
        Console.WriteLine("3. Check Package Records History");
        Console.WriteLine("4. Return Unknow packages to the post office.");
        Console.WriteLine("5. Log Out");
        int option = Convert.ToInt16(Console.ReadLine());
        return option;
    }

    // show records history
    public void DisplayRecords(DataTable tableRecords)
    {
        Console.WriteLine("---------------Table-------------------");
        foreach (DataRow row in tableRecords.Rows)
        {
            Console.WriteLine($"Unit_Number: {row["unitNumber"]} \t Resident_Name: {row["residentName"]} \t Post_Agency:{row["postServiceAgency"]}");
        }
    }
    // show unknow package
    public void DisplayUnknow(DataTable tableRecords)
    {
        Console.WriteLine("---------------Table-------------------");
        foreach (DataRow row in tableRecords.Rows)
        {
            Console.WriteLine($"Owner_Name: {row["packageOwner"]} \t Post_Agency: {row["postServiceAgency"]} \t Delivery_Date:{row["deliveryDate"]}");
        }
    }
}