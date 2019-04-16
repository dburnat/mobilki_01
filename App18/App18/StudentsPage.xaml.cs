using App18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App18
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage
    {
        private Teacher teacher;
        private bool isSelectable;
        private List<Student> studentsSelected;

        public StudentsPage(Teacher teacher)
        {
            this.teacher = teacher;
            studentsSelected = new List<Student>();
            isSelectable = true;
            InitializeComponent();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var student = e.Item as Student;

            if (!isSelectable)
            {
                if (await DisplayAlert($"{student.FirstName} {student.LastName}", $"Ocena: {student.Grade}, Ur.{student.Birthday.ToLongDateString()}. Czy przejść do edycji?", "Tak", "Nie"))
                {
                    await Navigation.PushAsync(new StudentEditPage(teacher, student)); //tryb edycji studenta
                                                                                       //Deselect Item
                    ((ListView)sender).SelectedItem = null;
                }
            }
            else
            {
                if (studentsSelected.Contains(student))
                {
                    studentsSelected.Remove(student);
                }
                else
                {
                    studentsSelected.Add(student);
                }
                btnSelect.Text = $"Remove students ({studentsSelected.Count()})";
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new StudentEditPage(teacher));

            //Random r = new Random();
            //var student = new Student()
            //{
            //    FirstName = "inż. Testowy",
            //    LastName = r.Next(1, 1000).ToString(),
            //    Grade = 2,
            //    TeacherID = teacher.ID
            //};

           // await App.LocalDB.SaveItem(student);
            await RefreshData();
        }

        private async Task RefreshData()
        {
            var students = await App.LocalDB.GetAll<Student>();
            students.RemoveAll(s => s.TeacherID != teacher.ID);
            MyListView.ItemsSource = students;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RefreshData();
        }

        private async void BtnSelect_Clicked(object sender, EventArgs e)
        {
            if (isSelectable)
            {
                if (studentsSelected.Any())             //jakikolwiek rekord
                {
                    foreach (var item in studentsSelected)
                    {
                        await App.LocalDB.DeleteItem(item);
                    }
                    studentsSelected.Clear();
                    await DisplayAlert("Sukces", "Usunięto rekordy", "OK");
                    await RefreshData();
                }
            }
           
            isSelectable = !isSelectable;
            btnSelect.Text = isSelectable ? "Select students" : "Remove students";
        }
    }
}
