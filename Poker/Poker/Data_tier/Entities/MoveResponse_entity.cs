using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier.Entities
{
    class MoveResponse_entity
    {
        public enum status { OK, GAME_FAILURE, FAILED_TO_SAVE, FAILED_TO_LOAD_GAME }

        public status moveStatus;

        public status MoveStatus
        {
            get
            {
                return moveStatus;
            }

            set
            {
                moveStatus = value;
            }
        }

        public MoveResponse_entity()
        {

        }
    }
}
