using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class SpiritBombSystem : JobComponentSystem {

    [ComputeJobOptimization]


    // 
    struct SpiritBombJob : IJobProcessComponentData<Position, BombEnergy>
    {
    public float energySpeed;
        public float3 bombPos;
        public bool charged;

        public void Execute(ref Position p, [ReadOnly] ref BombEnergy bE)
        {
            float3 dest = bombPos + bE.destOffset;

            // 
            if (!charged)
            {
                p.Value += (math.normalize(dest - p.Value))*energySpeed/10;
            }
            else
            {
                p.Value += (math.normalize(dest - p.Value))*.1f;
            }
        }
    }

    // 
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        SpiritBombJob sbJob = new SpiritBombJob
        {
            energySpeed = ctrl.eSpd,
            bombPos = ctrl.bPos,
            charged = ctrl.fullyCharged,
        };

        JobHandle handle = sbJob.Schedule(this, 64, inputDeps);

        return handle;
    }
}
