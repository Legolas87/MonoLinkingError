using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App12
{
  // Learn more about making custom code visible in the Xamarin.Forms previewer
  // by visiting https://aka.ms/xamarinforms-previewer
  [DesignTimeVisible(false)]
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }

    public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
    {
      var member = expression.Body as MethodCallExpression;

      if (member != null)
        return member.Method;

      throw new ArgumentException("Expression is not a method", "expression");
    }

    private void Button_OnClicked(object sender, EventArgs e)
    {
      try
      {
        MethodInfo mi = GetMethodInfo<MainPage>(xx => xx.ff(6));
        var paramInfo = mi.GetParameters()[0];

        var vals = paramInfo.GetCustomAttributes(true);

        DisplayAlert("count", vals.Count().ToString(), "OK");
      }
      catch(Exception ex)
      {
        DisplayAlert("exception", ex.Message, "OK");
      }
    }

    public  int ff([DefaultContextSource(typeof(int), 161)] int port)
    {
      return port;
    }
  }

  [AttributeUsage(AttributeTargets.Parameter), Serializable]
  public sealed class DefaultContextSourceAttribute : System.Attribute
  {

    #region constructors

    /// <summary>
    /// Konstruktor
    /// </summary>
    public DefaultContextSourceAttribute(Type type, object defaultValue)
    {
      Type = type;

      var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
      if (nullableUnderlyingType != null)
      {
        if (nullableUnderlyingType != typeof(string) && !nullableUnderlyingType.IsValueType)
          return; //z.B.: Nullable(record) fliegt raus, Nullable(int) nicht
      }
      if (type != typeof(string) && !type.IsValueType && nullableUnderlyingType == null) return;

      if (defaultValue == null && type.IsValueType)
        defaultValue = Activator.CreateInstance(type);
      DefaultValue = defaultValue;
    }

    #endregion

    #region properties

    /// <summary>
    /// Typ des Objects im IAppContext
    /// </summary>
    public Type Type { get; private set; }

    /// <summary>
    /// ID des Objects im IAppContext
    /// </summary>


    /// <summary>
    /// Wert des Defaults
    /// </summary>
    public object DefaultValue { get; private set; }

    #endregion


  }
}
