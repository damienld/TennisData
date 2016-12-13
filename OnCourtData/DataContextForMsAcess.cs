using System.Data.OleDb;
using System.Data;
using System;
using System.Data.Linq.Mapping;
[System.Data.Linq.Mapping.Provider(typeof(System.Data.Linq.SqlClient.Sql2000Provider))]
[System.Data.Linq.Mapping.DatabaseAttribute()]
public partial class DataContextForMsAccess : System.Data.Linq.DataContext
{
    private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();


    /*public DataContextForMsAccess() :
        base(global::LinqExp1.Properties.Settings.Default.cnwin_mainConnectionString, mappingSource)
    {
        OnCreated();
    }*/

    public DataContextForMsAccess(string connection) :
        base(connection, mappingSource)
    {
        OnCreated();
    }

    public DataContextForMsAccess(System.Data.IDbConnection connection) :
        base(connection, mappingSource)
    {
        OnCreated();
    }

    public DataContextForMsAccess(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
        base(connection, mappingSource)
    {
        OnCreated();
    }

    public DataContextForMsAccess(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
        base(connection, mappingSource)
    {
        OnCreated();
    }


    void OnCreated()
    { }

    /*
    void UpdateCustomer(Customer c)
    {
        this.ExecuteCommand("UPDATE [Customers] SET [Name]=@p1, [Description]=@p2, [Version]=@p3 WHERE [ID]=@p0", c.ID, c.Name);
    }

    void InsertCustomer(Customer c)
    {
        IDbCommand cmd;

        string query = "INSERT INTO [Customers] ([Name], [Description], [Version]) VALUES (@p0, @p1, @p2)";

        cmd = this.Connection.CreateCommand();
        cmd.Transaction = this.Transaction;
        cmd.CommandText = query;
        cmd.Parameters.Add(new OleDbParameter("p0", c.Name));
        cmd.Parameters.Add(new OleDbParameter("p1", c.Description));
        cmd.Parameters.Add(new OleDbParameter("p2", c.Version));
        cmd.ExecuteNonQuery();

        cmd = this.Connection.CreateCommand();
        cmd.Transaction = this.Transaction;
        cmd.CommandText = "SELECT @@IDENTITY";
        c.ID = Convert.ToInt32(cmd.ExecuteScalar());
    }


    


    public System.Data.Linq.Table Customers
    {
        get
        {
            return this.GetTable();
        }
    }


    ///////////////////////MAIN
    Customer newCust = new Customer() { Name = "n", Description = "d" };
    db.Customers.InsertOnSubmit(newCust);

    try
    {
    db.SubmitChanges();
    }
    catch (Exception err)
    {
    MessageBox.Show(err.Message);
    }

    MessageBox.Show(newCust.ID.ToString());
 */
   
}