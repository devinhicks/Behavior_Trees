using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : Kinematic
{
    GameObject mouse;

    public Door door;
    public GameObject cheese;
    Arrive moveType;

    // Start is called before the first frame update
    void Start()
    {
        moveType = new Arrive();
        moveType.character = this;
        moveType.target = target;

        mouse = this.gameObject;
    }

    public void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            RunTree();
        }

        // give character movement
        
    }

    public void RunTree()
    {
        // build tree to get to cheese
        Task getCheese = buildTree();
        getCheese.run();

        SteeringOutput mySteering = new SteeringOutput();
        mySteering.linear = moveType.getSteering().linear;
        base.Update();
    }

    public Task buildTree()
    {
        List<Task> tasks = new List<Task>();

        // if !locked, open door
        Task checkLock = new doorUnlocked(door);
        Task waitASec = new Wait(1f);
        Task openDoor = new OpenDoor(door);
        tasks.Add(checkLock);
        tasks.Add(waitASec);
        tasks.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(tasks);

        // barge it
        tasks = new List<Task>();
        Task checkDoorClosed = new doorClosed(door);
        Task bargeIt = new BargeDoor(door);
        tasks.Add(checkDoorClosed);
        tasks.Add(waitASec);
        tasks.Add(bargeIt);
        Sequence bargeClosedDoor = new Sequence(tasks);

        // ways to open a closed door
        tasks = new List<Task>();
        tasks.Add(openUnlockedDoor);
        tasks.Add(bargeClosedDoor);
        Selector openIt = new Selector(tasks);

        // get the cheese when door closed
        tasks = new List<Task>();
        Task toDoor = new MoveToDoor(mouse, door);
        Task toTarget = new MoveToTarget(mouse, cheese);
        tasks.Add(toDoor);
        tasks.Add(waitASec);
        tasks.Add(openIt);
        tasks.Add(waitASec);
        tasks.Add(toTarget);
        Sequence reachTargetBehindClosedDoor = new Sequence(tasks);

        // get the cheese when door is open
        tasks = new List<Task>();
        Task checkDoorOpen = new doorOpen(door);
        tasks.Add(checkDoorOpen);
        tasks.Add(toTarget);
        Sequence reachTargetBehindOpenDoor = new Sequence(tasks);

        // get that cheese
        tasks = new List<Task>();
        tasks.Add(reachTargetBehindOpenDoor);
        tasks.Add(reachTargetBehindClosedDoor);
        Selector reachTarget = new Selector(tasks);

        return reachTarget;
    }
}
