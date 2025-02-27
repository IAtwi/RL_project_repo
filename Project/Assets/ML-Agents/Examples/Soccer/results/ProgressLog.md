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
- **Final ELO**: 1208.563
- **Rewards**:
  - **Team Rewards**:
    - **Goal (Team Score)**: +1 - Time Passed / MaxSteps
  - **Agent Rewards**:
    - **Ball Touch**: None
    - **Passing to Teammates (Agent Score)**: +0.2
    - **Spacing Out (Agent Score)**: +0.1
- **Notes**: Some agents just stuck to the goal and never moved. There is a lot to improve.
