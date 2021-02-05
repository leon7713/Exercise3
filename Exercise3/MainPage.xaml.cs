using System.IO;
using System.Xml.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading;

namespace Exercise3
{

   public sealed partial class MainPage : Page
   {
      public SerializationUD serialization;

      public MainPage()
      {
         this.InitializeComponent();
         serialization = new SerializationUD();
      }

      private void LoadBTN_click(object sender, RoutedEventArgs e)
      {
         //ThreadPool.QueueUserWorkItem(LoadProcess);
         UserDetails userData = serialization.DeserializeData();
         IdTB.Text = userData.Id.ToString();
         NameTB.Text = userData.FullName;
         AddressTB.Text = userData.Address;
      }

      private void SaveBTN_click(object sender, RoutedEventArgs e)
      {
         UserDetails userDetails = new UserDetails(int.Parse(IdTB.Text), NameTB.Text, AddressTB.Text);
         ThreadPool.QueueUserWorkItem(SaveProcess, userDetails);
         IdTB.Text = string.Empty;
         NameTB.Text = string.Empty;
         AddressTB.Text = string.Empty;
      }

      public void SaveProcess(object callback)
      {
         serialization.SerializeData((UserDetails)callback);
      }

      public void LoadProcess(object callback)
      {
         UserDetails userData = serialization.DeserializeData();
         //IdTB.Text = userData.Id.ToString();
         //NameTB.Text = userData.FullName;
         //AddressTB.Text = userData.Address;
      }
   }

   public class UserDetails
   {
      [XmlAttribute]
      public int Id;
      public string FullName, Address;

      public UserDetails() { }

      public UserDetails(int id, string name, string address)
      {
         Id = id;
         FullName = name;
         Address = address;
      }
   }

   public class SerializationUD
   {
      //Serialization class

      //data members
      FileStream stream;
      XmlSerializer xmlSerializer;

      //c'tor
      public SerializationUD()
      {
         xmlSerializer = new XmlSerializer(typeof(UserDetails));
      }

      //methods
      internal void SerializeData(UserDetails userDetails)
      {
         using (stream = new FileStream($@"{Windows.Storage.ApplicationData.Current.LocalFolder.Path}\UserDetails.xml",
            FileMode.Create))
         {
            //Thread.Sleep(5000);
            xmlSerializer.Serialize(stream, userDetails);
         }
      }


      internal UserDetails DeserializeData()
      {
         using (stream = new FileStream($@"{Windows.Storage.ApplicationData.Current.LocalFolder.Path}\UserDetails.xml",
            FileMode.OpenOrCreate))
         {
            UserDetails userDetails = (UserDetails)xmlSerializer.Deserialize(stream);
            return userDetails;
         }
      }
   }
}
