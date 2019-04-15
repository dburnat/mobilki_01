using App18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App18
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentEditPage : ContentPage
	{
        private Teacher teacher;
        private Student student;
		public StudentEditPage (Teacher teacher , Student student = null)
		{
            this.teacher = teacher;
            this.student = student;
			InitializeComponent ();
            labelTeacherName.Text = teacher.FirstName + " " + teacher.LastName;

            if(student != null)
            {
                entryFirstName.Text = student.FirstName;
                entryLastName.Text = student.LastName;
                entryDegree.Text = student.Grade.ToString();
                entryDate.Date = student.Birthday;
            }
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            
            
            var newStudent = new Student()
            {
                FirstName = entryFirstName.Text,
                LastName = entryLastName.Text,
                Grade = int.Parse(entryDegree.Text),
                Birthday = entryDate.Date,
                TeacherID = teacher.ID
            };

            if(student != null)
            {
                newStudent.ID = student.ID;
            }
            await App.LocalDB.SaveItem(newStudent);
            await DisplayAlert("Sukces", "Udało się dodać studenta" , "OK");
            await Navigation.PopAsync();
        }
    }
}