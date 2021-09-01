using Assets.Scripts.Common;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    class Mail : MonoBehaviour, IContextAction
    {
        public const string xzers_in_work_room_who = @"null";
        public const string xzers_in_work_room_from = @"Para";
        public const string xzers_in_work_room_date = @"42/-15/14010 -450:20";
        public const string xzers_in_work_room_title = @"Oi filho, é a mamãe";
        public const string xzers_in_work_room_message = @"Estou tentanto há horas falar com vocês. Estão dizendo que vocês perderam as comunicações de novo. Nos deram esses dispositivos para se comunicarmos com vocês, que bom que se importam com a gente. Quero que saiba que a mamãe ama vocês. Tentei contato com seu irmão mais cedo, diga a ele que a mamãe o ama. Estamos esperando vocês.";
        public const string xzers_in_work_room_status = @"Não foi possível enviar a mensagem, tente novamente.";
    



        public Collider trigger;
        public GUIMail GUIMail;
        public ParticleSystem particle;

        public void Action()
        {
            if (GUIMail != null)
            {
                GUIMail.who.text = xzers_in_work_room_who;
                GUIMail.from.text = xzers_in_work_room_from;
                GUIMail.date.text = xzers_in_work_room_date;
                GUIMail.title.text = xzers_in_work_room_title;
                GUIMail.message.text = xzers_in_work_room_message;
                GUIMail.statusText.text = xzers_in_work_room_status;

                GUIMail.ShowUI();

                if (particle != null)
                    particle.Stop();
            }
               
        }

        public void SetObjectTriggable()
        {
            gameObject.tag = Constants.ActionTriggerTag;
            gameObject.layer = LayerMask.NameToLayer(Constants.TriggersLayer);

            trigger.isTrigger = true;
        }
    }
}
