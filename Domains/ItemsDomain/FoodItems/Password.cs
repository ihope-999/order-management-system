namespace project1.Domains.ItemsDomain.FoodItems
{

    public enum Password
    {
        VeryWeak,
        Weak,
        Medium,
        Strong,
        VeryStrong
    }

    public record PasswordStrength(int score, bool passed);

}
