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
            //btnDelete.IsVisible = 
            labelTeacherName.Text = teacher.FirstName + " " + teacher.LastName;

            if(student != null)
            {
                entryFirstName.Text = student.FirstName;
                entryLastName.Text = student.LastName;
                entryDegree.Text = student.Grade.ToString();
                entryDate.Date = student.Birthday;
                btnDelete.IsVisible = true;
            }
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            bool isItEdit = false;
            
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
                isItEdit = true;
                newStudent.ID = student.ID;
            }
            await App.LocalDB.SaveItem(newStudent);
            if(isItEdit)
                await DisplayAlert("Sukces", "Udało się edytować dane studenta" , "OK");
            else
                await DisplayAlert("Sukces", "Udało się dodać studenta", "OK");
            await Navigation.PopAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await DeleteStudent();
        }

        private async Task DeleteStudent()
        {

            if (student != null)
            {
                await App.LocalDB.DeleteItem(student);
                await DisplayAlert("Sukces", "Usunięto studenta", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}