using System.Collections.Generic;
using UnityEngine;
using Video.Youtube;

namespace UI.Video
{
    public class SlotView : View<YoutubeViewModel>
    {
        public Transform slotParent;
        public SlotComponent slotDummy;

        private List<SlotComponent> slotComponents;

        protected override void Awake()
        {
            base.Awake();

            slotComponents = GetUIComponents<SlotComponent>();
            slotComponents.ForEach(x => x.OnEventProps = Event);
        }

        protected override void Create()
        {
            var obj = Instantiate(slotDummy, slotParent) as SlotComponent;
            obj.OnEventProps = Event;
            slotComponents.Add(obj);
        }

        public override void Upsert()
        {
            var videos = ServerInfo.Videos;

            slotComponents.ForEach(x => x.Empty());

            videos.ForEach(async (x, i) =>
            {
                if (i >= slotComponents.Count)
                    Create();   //슬롯생성

                await slotComponents[i].Upsert(i, x);
            });
        }

        protected override void Event(bool isSelected, Props<YoutubeViewModel> props)
        {
            selectProps.Clear();

            if (isSelected)
                selectProps.Add(props);

            base.Event(isSelected, props);
        }
    }
}