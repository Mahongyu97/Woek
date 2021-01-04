@copy ..\Client\Data\CharacterDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy ..\Client\Data\MapDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy ..\Client\Data\LevelUpDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy ..\Client\Data\SpawnRuleDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy ..\Client\Data\NpcDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy ..\Client\Data\TeleporterDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\

@copy ..\Lib\Common\bin\Debug\Common.dll ..\Client\Assets\References\
@copy ..\Lib\Common\bin\Debug\log4net.dll ..\Client\Assets\References\
@copy ..\Lib\Common\bin\Debug\protobuf-net.dll ..\Client\Assets\References\
@copy ..\Lib\Common\bin\Debug\Protocol.dll ..\Client\Assets\References\

pause