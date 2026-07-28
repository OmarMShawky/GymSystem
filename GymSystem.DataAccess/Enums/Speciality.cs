using System.ComponentModel.DataAnnotations;

namespace GymSystem.DataAccess.Enums;

public enum Specialty
{
    [Display(Name = "Personal Training")]
    PersonalTraining = 1,

    [Display(Name = "Bodybuilding")]
    Bodybuilding = 2,

    [Display(Name = "CrossFit")]
    CrossFit = 3,

    [Display(Name = "Calisthenics")]
    Calisthenics = 4,

    [Display(Name = "Cardio Fitness")]
    CardioFitness = 5,

    [Display(Name = "Yoga")]
    Yoga = 6,

    [Display(Name = "Pilates")]
    Pilates = 7,

    [Display(Name = "Nutrition Coaching")]
    NutritionCoaching = 8
}
