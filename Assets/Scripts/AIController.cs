using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Unit {

    public float shootInterval;
    public float range;

	public float coconut;

	float runningRange = 50.0f;

	public GameObject coconutPrefab;
	public float shootForce;


    //A shootOffset variable so the AI doesn't shoot at the feet, but at the chest
    private Vector3 shootOffset = new Vector3(0, 1.5f, 0);

    private NavMeshAgent agent;

    //An enum allows us to create a variable type (in this case 'State')
    //This variable type can only have the values that we declare inside the enum
    private enum State
    {
        Idle,
        MovingToOutpost, 
        ChasingEnemy,
		Dancing,
		Fleeing
    }

    private State currentState;



    private Outpost currentOutpost;
    private Unit currentEnemy;
	private Unit currentGathering; 




    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        SetState(State.Idle);
    }

    private void SetState (State newState)
    {
        //This will stop all Coroutines on this MonoBehaviour (so only this object, not other ones)
        //We do this to ensure we are never in two states at the same time
        StopAllCoroutines();
        currentState = newState;
        switch (currentState)
        {
				// Idle state 
                case State.Idle:
                StartCoroutine(OnIdle());
                break;
			    // Moving to outpost state 
                case State.MovingToOutpost:
                StartCoroutine(OnMovingToOutpost());
                break;

				// Added fleeing state 
		    	case State.Fleeing:
				StartCoroutine (OnFleeing());
				break;
			     // Chasing Enemy state 
                case State.ChasingEnemy:
                StartCoroutine(OnChasingEnemy());
                break;

			Destroy(GetComponent<Collider>());

        }
    }




	// STATE ONE

    IEnumerator OnIdle ()
    {
        //This is called once, when I enter the OnIdle Coroutine

        while (currentOutpost == null)
        {
            LookForOutpost();
            //yield return null pauses the execution of this method for one frame
            yield return null;

            //This is called every frame, until StopAllCoroutines is called.
        }

        SetState(State.MovingToOutpost);
    }

	// STATE TWO

    IEnumerator OnMovingToOutpost()
    {
        //This is called once, when I enter the OnMovingToOutpost Coroutine
        //We set our target once, the outpost doesn't move so there is no need to recalculate the path
        agent.SetDestination(currentOutpost.transform.position);

        //As soon as the captureValue is 1 and the outpost belongs to my team, I move on
        while (!(currentOutpost.captureValue == 1 && currentOutpost.team == team))
        {
            LookForEnemy();


            //yield return null pauses the execution of this method for one frame
            yield return null;
            //This is called every frame, until StopAllCoroutines is called.
        }

        // When the outpost is fully captured, we can go back to Idle
        currentOutpost = null;

		// Go back to idle state
		SetState(State.Idle);
    }



	IEnumerator Dancing() {

			anim.SetTrigger ("Jump");



		yield return null;

		SetState(State.Fleeing);


	}



	// STATE THREE

	// Add the fleeing state 
	IEnumerator OnFleeing()
	{



			
			anim.SetTrigger ("Jump");


			// Find the tree in the map and go to that prefab in this case in the fleeing state we set the find object type to a tree where the bears that have completed the above states will go there
			// This is a simpe way of making the bears flee the main scene however I do think there is a better way of doing this if this game were to be published.
			Tree player = FindObjectOfType<Tree> ();
			// Set the destination 
			agent.SetDestination (player.transform.position);



			// Lets destroy the AIController so that when a bear completes the states above and leaves the scene we dont continue to allow the bears to shoot each other and use game memory.
			Destroy (GetComponent<AIController> ());
			// I tested this without destroying the AIController in this added state and without destroying the 
			// AIController the game gets really slow after a minute as the bears that flee are still using game memory even after they left the main area aw the AIController was not destroyed.


	

	

		    // Return state 
			yield return null;
		//}

		SetState(State.ChasingEnemy);
	
	}


	// STATE FOUR

    IEnumerator OnChasingEnemy()
    {
        float shootTimer = 0;

        while (currentEnemy.health > 1)
        {
            shootTimer += Time.deltaTime;
            //If our enemy is out of range, chase him down
			if (Vector3.Distance(transform.position, currentEnemy.transform.position) > range)
            {
                agent.SetDestination(currentEnemy.transform.position);
            } else
            {
                //We clear the current path, so the AI is not moving and shooting at the same time
                agent.ResetPath();

                if (shootTimer >= shootInterval)
                {
                    shootTimer = 0;
                    //We shoot at the enemies position, with a bit of offset so it shoots at the chest, not feet
                    ShootLasers(currentEnemy.transform.position + shootOffset, currentEnemy.transform);
                }
            }

            yield return null;
        }

		// When a bear captures the flag and kills another bear change the state to fleeing so that the bear will run away
		SetState(State.Dancing);
    }






    void LookForEnemy ()
    {
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, range);
        foreach(Collider c in surroundingColliders)
        {
            Unit otherUnit = c.GetComponent<Unit>();
            if (otherUnit != null && otherUnit.team != team && otherUnit.health > 0 
                && CanSee(otherUnit.transform.position + shootOffset, otherUnit.transform))
            {
                //Save the unit that we will start chasing
                currentEnemy = otherUnit;
                SetState(State.ChasingEnemy);
                //When we found our enemy, we stop looking
                return; //putting break; here does the same thing
            }
        }
    }

    void LookForOutpost ()
    {
        //Create a random index integer
        int r = Random.Range(0, GameManager.instance.outposts.Length);
        currentOutpost = GameManager.instance.outposts[r];
    }

	void LookForRun () 
	{

	
	

	}



    // Update is called once per frame
    void Update () {
        //PlayerController player = FindObjectOfType<PlayerController>();
        //agent.SetDestination(player.transform.position);

        //We send the current speed of the agent every frame, to display the movement animation
        anim.SetFloat("VerticalSpeed", agent.velocity.magnitude);
	}

    protected override void Die()
    {
        base.Die();

	


		// Stop all the coroutines except for the fleeing coroutine so that the dead players will be removed from the main area of the map.
		StopCoroutine(OnChasingEnemy()); 
		StopCoroutine (OnIdle());
		StopCoroutine (OnMovingToOutpost());

		//agent.SetDestination(player.transform.position);
        //Stops the NavmeshAgent from moving
        agent.Stop();
        Destroy(GetComponent<Collider>());
    }
}
