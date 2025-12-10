using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Items.Commands.CreateItem.Dtos
{
    public record CreateItemRequest(Guid ProductId, string Name);
}
