﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simuro5v5
{
    public class ObjectManager
    {
        public GameObject ballObject { get; private set; }
        public GameObject[] blueObject { get; private set; }
        public GameObject[] yellowObject { get; private set; }

        public ControlBall ballComponent { get; private set; }
        public ControlRobot[] blueComponent { get; private set; }
        public ControlRobot[] yellowComponent { get; private set; }

        public MatchInfo OutputMatchInfo { get; private set; }

        public ObjectManager() { }

        public ObjectManager(MatchInfo matchInfo)
        {
            RebindMatchInfo(matchInfo);
            RebindObject();
        }

        /// <summary>
        /// 重新绑定一个用来输出的MatchInfo
        /// </summary>
        /// <param name="matchInfo"></param>
        public void RebindMatchInfo(MatchInfo matchInfo)
        {
            OutputMatchInfo = matchInfo;
        }

        /// <summary>
        /// 重新绑定场景中的对象
        /// </summary>
        public void RebindObject()
        {
            ballObject = GameObject.Find("Ball");
            blueObject = new GameObject[5] {
                GameObject.Find("Blue0"),
                GameObject.Find("Blue1"),
                GameObject.Find("Blue2"),
                GameObject.Find("Blue3"),
                GameObject.Find("Blue4")
            };
            yellowObject = new GameObject[5] {
                GameObject.Find("Yellow0"),
                GameObject.Find("Yellow1"),
                GameObject.Find("Yellow2"),
                GameObject.Find("Yellow3"),
                GameObject.Find("Yellow4"),
            };

            ballComponent = ballObject.GetComponent<ControlBall>();
            blueComponent = new ControlRobot[5] {
                blueObject[0].GetComponent<ControlRobot>(),
                blueObject[1].GetComponent<ControlRobot>(),
                blueObject[2].GetComponent<ControlRobot>(),
                blueObject[3].GetComponent<ControlRobot>(),
                blueObject[4].GetComponent<ControlRobot>(),
            };
            yellowComponent = new ControlRobot[5] {
                yellowObject[0].GetComponent<ControlRobot>(),
                yellowObject[1].GetComponent<ControlRobot>(),
                yellowObject[2].GetComponent<ControlRobot>(),
                yellowObject[3].GetComponent<ControlRobot>(),
                yellowObject[4].GetComponent<ControlRobot>(),
            };
        }

        public void SetToDefault()
        {
            RevertScene(MatchInfo.DefaultMatch);
        }

        public void SetBlueWheels(WheelInfo ws)
        {
            for (int i = 0; i < 5; i++)
            {
                OutputMatchInfo.BlueRobot[i].velocityLeft = ws.Wheels[i].left;
                OutputMatchInfo.BlueRobot[i].velocityRight = ws.Wheels[i].right;
                blueComponent[i].SetWheelVelocity(ws.Wheels[i]);
            }
        }

        public void SetYellowWheels(WheelInfo ws)
        {
            for (int i = 0; i < 5; i++)
            {
                OutputMatchInfo.YellowRobot[i].velocityLeft = ws.Wheels[i].left;
                OutputMatchInfo.YellowRobot[i].velocityRight = ws.Wheels[i].right;
                yellowComponent[i].SetWheelVelocity(ws.Wheels[i]);
            }
        }

        public void SetBluePlacement(PlacementInfo sInfo)
        {
            for (int i = 0; i < 5; i++)
            {
                blueComponent[i].SetPlacement(sInfo.Robot[i]);
            }
        }

        public void SetBluePlacement(Robot[] robots)
        {
            for (int i = 0; i < 5; i++)
            {
                blueComponent[i].SetPlacement(robots[i]);
            }
        }

        public void SetYellowPlacement(PlacementInfo sInfo)
        {
            for (int i = 0; i < 5; i++)
            {
                yellowComponent[i].SetPlacement(sInfo.Robot[i]);
            }
        }

        public void SetYellowPlacement(Robot[] robots)
        {
            for (int i = 0; i < 5; i++)
            {
                yellowComponent[i].SetPlacement(robots[i]);
            }
        }

        public void SetBallPlacement(PlacementInfo sInfo)
        {
            ballComponent.SetPlacement(sInfo.Ball);
        }

        public void SetBallPlacement(Ball ball)
        {
            var b = ball;
            b.Normalize();
            ballComponent.SetPlacement(b);
        }

        public void SetStill()
        {
            for (int i = 0; i < 5; i++)
            {
                blueComponent[i].SetStill();
                yellowComponent[i].SetStill();
            }
            ballComponent.SetStill();
        }

        /// <summary>
        /// 从指定MatchInfo还原场景
        /// </summary>
        /// <param name="matchInfo"></param>
        public void RevertScene(MatchInfo matchInfo)
        {
            for (int i = 0; i < 5; i++)
            {
                blueComponent[i].Revert(matchInfo.BlueRobot[i]);
                yellowComponent[i].Revert(matchInfo.YellowRobot[i]);
            }
            ballComponent.Revert(matchInfo.Ball);
            OutputMatchInfo.Update(matchInfo);
        }

        /// <summary>
        /// 从场景更新到绑定的MatchInfo中
        /// </summary>
        public void UpdateFromScene()
        {
            OutputMatchInfo.UpdateEntity(ballObject, blueObject, yellowObject);
        }

        /// <summary>
        /// 继续运行世界
        /// </summary>
        public void Resume()
        {
            Time.timeScale = Const.TimeScale;
        }

        /// <summary>
        /// 暂停世界
        /// </summary>
        public void Pause()
        {
            Time.timeScale = 0.0f;
        }
    }
}
