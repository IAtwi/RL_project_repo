# Training Progress Log

This file serves as a record of the training progress. We will write any main modifications before each model update here, so that we can keep track of the changes made to the model and its effects.

## Model Updates

Run IDs are used as the heading for the logs

### FirstRunWithModifiedRays

- **Date**: 2025-02-27
- **Description**: First run with modified rays. The front rays span across 80 degrees and have two additional rays. The model is trained with 8 agents in the environment. It serves purely as a baseline for future comparisons and a safe check that everything is running correctly.

### FirstSeriousRun

- **Title**: SoccerFours0.1
- **Date**: 2025-02-27
- **Description**: First serious run with 8 agents in the environment. The model is trained with the same configuration as the previous run. Rewards for blocking shots and staying in formation have been disabled.
- **Steps**: 1.2M
- **Training Time**: 50m
- **Initiated From**: scratch
- **Final ELO**: 1208.563
- **Rewards**:
  - **Team Rewards**:
    - **Goal (Team Score)**: +1 - Time Passed / MaxSteps
  - **Agent Rewards**:
    - **Ball Touch**: None
    - **Passing to Teammates (Agent Score)**: +0.2
    - **Spacing Out (Agent Score)**: +0.1
- **Notes**: Some agents just stuck to the goal and never moved. There is a lot to improve.

### SoccerFours0.2

- **Title**: SoccerFours0.2
- **Date**: 2025-02-28
- **Description**: Second run with 8 agents in the environment. Number of fields was increased to 16 (was 8) which greatly sped up the training time at the cost of additional resources. The model is trained with the same configuration as the previous run. Rewards for passing and positioning where greatly decreased and proportionate to the number of max steps.
- **Steps**: 2.1M
- **Training Time**: 1h 46m
- **Initiated From**: scratch
- **Peak ELO**: 1227 at 1.5M steps
- **Final ELO**: 1208.563
- **Best Model**: SoccerTwos-1499245.onnx
- **Rewards**:
  - **Team Rewards**:
    - **Goal (Team Score)**: +1 - Time Passed / MaxSteps
    - **Ball Touch**: None
  - **Agent Rewards**:
    - **Passing to Teammates**: +5 / MaxSteps
    - **Spacing Out**: +1 / MaxSteps
- **Notes**: Patterns of agents just sticking to a goal were noticed again, noticeably it's often two agents, with each being at one end of the goal. There are suspicions that they are farming the passing reward by just staying in that place even if they are doing nothing, so we might disable the passing reward for the next run and see how that goes.

### SoccerFours0.3

- **Title**: SoccerFours0.3
- **Date**: 2025-02-28
- **Description**: No changes to the configuration. This was to see how the agents will behave without all the custom rewards.
- **Steps**: 1.2M
- **Training Time**: 1h 13m
- **Initiated From**: scratch
- **Peak ELO**: N/A
- **Final ELO**: N/A
- **Best Model**: N/A
- **Rewards**:
  - **Team Rewards**:
    - **Goal (Team Score)**: +1 - Time Passed / MaxSteps
  - **Notes**: There was no learning, agents just stood still.

### SoccerFours0.4

- **Title**: SoccerFours0.4
- **Date**: 2025-02-28 - 2025-03-01
- **Description**: First promising results, you just need a LOT of training time to get something, unfortunately terminal crashed mid training, so it was cut abruptly which affects any chances of resuming the training. Effort was put to salvage the best model possible.
- **Steps**: 20M
- **Training Time**: 10h 30m
- **Initiated From**: scratch
- **Peak ELO**: 1538
- **Final ELO**: 1538
- **Best Model**: SoccerTwos-19499943.onnx (should be, but since the training crashed in the middle this is the one with the best performance)
- **Rewards**:
  - **Team Rewards**:
    - **Goal (Team Score)**: +1 - Time Passed / MaxSteps
    - **Spacing Out**: +1 / MaxSteps
  - **Agent Rewards**:
    - **Ball Touch**: 0.2f * (+5f / MaxSteps)
    - **Passing to Teammates**: +5 / MaxSteps
  - **Notes**: There was a big jump in learning after 6 hours of training, it was 1250 ELO at 6 hours, jumped straight to 1400 at 8 hours and was consistently improving afterwards, up to 1520.

### SoccerFours0.5

- **Title**: SoccerFours0.5
- **Date**: 2025-03-01 - 2025-03-02
- **Description**: First model to get built on top of a past model. We pushed training time very high, to see how good it can get. ELO was going up and down, between 1300 and going down all the way to 1100, despite those fluctuations, the actual performance of the agents was very good.
- **Steps**: 28.580M
- **Training Time**: 20h 37m
- **Initiated From**: SoccerFours0.4
- **Peak ELO**: 1320
- **Final ELO**: 1076
- **Best Model**: SoccerFours0.5.onnx
- **Rewards**:
  - **Team Rewards**:
    - **Goal (Team Score)**: +1 - Time Passed / MaxSteps
    - **Spacing Out**: +1 / MaxSteps
  - **Agent Rewards**:
    - **Ball Touch**: 0.2f * (+5f / MaxSteps)
    - **Passing to Teammates**: +5 / MaxSteps
  - **Notes**: The agents learned on their own to maintain their positions. To use their environment to their advantage, to cooperate and pass the ball to each other, and are very dynamic to their situations.
