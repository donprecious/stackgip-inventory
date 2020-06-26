namespace StackgipInventory.Config
{
   public interface IConfig
   {
       string MSSqlConnection();
       string GetValue(string prop);
   }
}
