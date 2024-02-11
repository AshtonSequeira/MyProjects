using droneSim;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//initialises and updates the drone engine

public interface IEngine
{
    void initEngine();

    void updateEngine(Rigidbody _rb, droneInputs input, float _maxPower);
}
