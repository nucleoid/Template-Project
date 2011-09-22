
namespace MvpRestApiLib
{
    public interface IRestModel
    {
        /// <summary>
        /// Serializable model for REST requests
        /// </summary>
        object RestModel { get; }
    }
}
