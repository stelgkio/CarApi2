using MediatR;

namespace CarApi2.Application.Common
{
    public class BaseCqrsRequest<T> : IRequest<T>
    {
        protected BaseCqrsRequest()
        {
           
        }

        
    }
}
