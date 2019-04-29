CREATE TABLE [dbo].[Table]
(
    [MatchID] INT NOT NULL, 
    [HomeTeamMidfield] INT NOT NULL, 
    [HomeTeamRDefense] INT NOT NULL, 
    [HomeTeamCDefense] INT NOT NULL, 
    [HomeTeamLDefense] INT NOT NULL, 
    [HomeTeamRAttack] INT NOT NULL, 
    [HomeTeamCAttack] INT NOT NULL, 
    [HomeTeamLAttack] INT NOT NULL, 
    [AwayTeamMidfield] INT NOT NULL, 
    [AwayTeamRDefense] INT NOT NULL, 
    [AwayTeamCDefense] INT NOT NULL, 
    [AwayTeamLDefense] INT NOT NULL, 
    [AwayTeamRAttack] INT NOT NULL, 
    [AwayTeamCAttack] INT NOT NULL, 
    [AwayTeamLAttack] INT NOT NULL, 
    [HomeTeamGoals] INT NOT NULL, 
    [AwayTeamGoals] INT NOT NULL, 
    PRIMARY KEY ([MatchID])
)
