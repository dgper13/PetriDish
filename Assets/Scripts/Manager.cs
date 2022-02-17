using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public int PopulationSize;
    public int StartingCases;
    public int MinHealingTime;
    public int MaxHealingTime;
    public int MinSeverity;
    public int MaxSeverity;
    public float TransmissionRate;
    public bool Run = false;
    public GameObject AgentPrefab;
    public List<Agent> Agents = new List<Agent>();
    int boardSide;
    public float AgentScale = 0.75f;
    private List<int> PositionsOfInterest;

    private float time = 0.0f;
    public float interpolationPeriod = 0.01f;

    private Bounds board;
    private float minX;
    private float minY;
    private float width;
    private float height;
    
    public Text casesText;
    public Text deathsText;
    public Text deathRate;

    void Start()
    {
        board = gameObject.GetComponent<SpriteRenderer>().bounds;
        width = board.size.x;
        height = board.size.y;
        minX = board.min.x;
        minY = board.min.y;
    }

    private List<int> GetPositionsOfInterest(int index)
    {
        List<int> positions = new List<int>();

        if (index % boardSide > 0){
            positions.Add(index - 1); //left
        }

        if (index % boardSide + 1 < boardSide && index + 1 < PopulationSize){
            positions.Add(index + 1); //right
        }

        if (index - boardSide >= 0)
        {
            positions.Add(index - boardSide); //top
            if (index % boardSide > 0)
            {
                positions.Add(index - boardSide - 1); //topLeft
            }
            if (index % boardSide < boardSide - 1)
            {
                positions.Add(index - boardSide + 1); //topRight
            }
        }

        if (index + boardSide - 1 < PopulationSize)
        {
            positions.Add(index + boardSide - 1); //bottomLeft
        }

        if (index + boardSide < PopulationSize)
        {
            positions.Add(index + boardSide); //bottom
        }

        if (index + boardSide + 1 < PopulationSize)
        {
            positions.Add(index + boardSide + 1); //bottomRight
        }

        return positions;
    }

    void Step()
    {
        int cases = 0;
        int deaths = 0;
        int totalCases = 0;
    
        for (int index = 0; index < PopulationSize; index++)
        {
            if (Agents[index].status == HealthStatus.Healthy)
            {
                PositionsOfInterest = GetPositionsOfInterest(index);

                foreach (int position in PositionsOfInterest)
                {
                    if (Agents[position].status == HealthStatus.Infected)
                    {
                        if (Random.value < TransmissionRate/100)
                        {
                            Agents[index].status = HealthStatus.Infected;
                            break;
                        }
                    }
                }
            }
            else if (Agents[index].status == HealthStatus.Infected)
            {
                cases += 1;
            }
        

            if (Agents[index].status != HealthStatus.Dead)
            {
                Agents[index].Step();
            }
            else
            {
                deaths += 1;
            }
            
            if (Agents[index].status == HealthStatus.Dead ||
            Agents[index].status == HealthStatus.Immune ||
            Agents[index].status == HealthStatus.Infected)
            {
                totalCases += 1;
            }

        }
        casesText.text = "CASES: "  + totalCases;
        deathsText.text = "DEATHS: "  + deaths;

        if (totalCases == 0)
        {
            deathRate.text = "DEATH RATE: 0%";
        }
        else
        {
            deathRate.text = "DEATH RATE: " + 100*deaths/totalCases + "%";
        }        
    }

    public void Activate()
    {   
        boardSide = Mathf.CeilToInt(Mathf.Sqrt(PopulationSize));
        float sizeX = width / boardSide;
        float sizeY = height / boardSide;

        Vector3 topLeft = board.max + width * Vector3.left + new Vector3(sizeX / 2, -sizeY / 2, 0);

        float scaleFac = 1 / (float)boardSide * AgentScale;


        for (int index = 0; index < PopulationSize; index++)
        {
            int x = index % boardSide, y = index / boardSide;
            Agents.Add(Instantiate(AgentPrefab, topLeft + new Vector3(x * sizeX, -y * sizeY, 0), Quaternion.identity, transform).GetComponent<Agent>());
            Agents[index].transform.localScale *= scaleFac;
            
            Agents[index].MinHealingTime = MinHealingTime;
            Agents[index].MaxHealingTime = MaxHealingTime;
            Agents[index].MinSeverity = MinSeverity;
            Agents[index].MaxSeverity = MaxSeverity;
        }

        int j = 0;
        int pos;

        while (j < StartingCases && j < PopulationSize)
        {
            pos = Random.Range(0, PopulationSize);
            if (Agents[pos].status == HealthStatus.Healthy)
            {
                Agents[pos].status = HealthStatus.Infected;
                j++;
            }
        }

        Run = true;
    }

    void Update ()
    {   
        if (Run)
        {
            time += Time.deltaTime;
            if (time >= interpolationPeriod)
            {
                time = time - interpolationPeriod;
                Step();
            }
        }
    }
}
