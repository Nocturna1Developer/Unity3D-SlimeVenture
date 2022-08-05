
namespace CodeMonkey.HealthSystemCM {

    /// <summary>
    /// Interface to decouple objects that need to interact with the Health System from whatever object creates it.
    /// Especially useful if you're using the C# constructor method to create a Health System but still want the 
    /// built-in health bar to work with it.
    /// </summary>
    public interface IGetHealthSystem {

        HealthSystem GetHealthSystem();

    }

}