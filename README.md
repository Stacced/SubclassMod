## SubclassMod (v1.0.9) (EXILED-5.2.1)
This is a game modification for SCP:SL that adds to the game the ability to create and configure subclasses, as well as modify existing game roles. This modification will allow you to create your own subclasses and customize them as you like, up to replacing the HP level. Or you can customize the entire class by changing its name/prefix/postfix or overwriting the inventory for a particular class. Also support roleplay firstnames and secondnames for human classes and class d badges for d boiz.

## Installation
Download latest plugin release and put ``SubclassMod.dll`` inside your ``/EXILED/Plugins/`` folder. All features can be customized in config and translations. __EXILED-5.0.0 version required__.

## Commands

### ``force`` - Force player as subclass
Permission: ``scmod.force``
Usage: ``force <playerId/*> <subclassId>``

## Configs Management
Example of creating custom subclass and customizating roles (**Every new subclass should start from '-' and have unique ID**):
### Subclasses setup example
```yml
  # All subclasses placed in this list
  custom_subclasses:
    # SHOULD BE UNIQUE
  - id: 0
    # Maximum value of players that can be spawned on subclass (0 - unlimited)
    max_players: 0
    # Health value
    health: 100
    # Chance to be spawned as this role
    spawn_percent: 50
    # Player display nickname and just class name that will be showed on spawn
    name: Seniour Scientist 
    # Subclass description that will be showed on spawn
    description: Just a Seniour Scientist
    # Prefix that will be placed in player display nickname {PREFIX}NAME
    name_prefix: 'Dr.'
    # Postfix that will be placed in player display nickname NAME{POSTFIX}
    name_postfix: ''
    # Custom info that will be added to player on spawn
    custom_info: Area Seniour Scientist
    # Can be class spawned only using command
    forceclass_only: false
    # Method that will be used when plugin will select nickname for this role
    naming_method: Firstname/Signs/Nickname
    # The role on which the subclass will be based
    base_role: Scientist
    # Spawn position search method that will be used on player spawn. Variants: SpawnZone, SpawnPositions, SpawnRooms
    spawn_method: SpawnZone/SpawnPositions/SpawnRooms
    # Ammo that will be given to player on spawn
    ammo:
      Nato9: 10
      Nato556: 10
      Nato762: 10
    # Inventory override for subclass
    items:
    - Coin
    - KeycardResearchCoordinator
    - ParticleDisruptor
    # Values for spawn methods (ALL BELOW)
    spawn_zones:
    - Entrance
    spawn_rooms:
    - LczChkpA
    spawn_positions:
    - x: 0
      y: 0
      z: 0
```

### Role customization example
```yml
  # In this list will be written all classes customizations (ONLY ONE CUSTOMIZATION FOR ONE CLASS). Changes will be applied to all players that will be spawned as selected class.
  custom_roles_info:
    FacilityGuard:
      # Prefix that will be placed before nickname {PREFIX}NAME
      name_prefix: Dr.
      # Postfix that will be placed after nickname NAME{POSTFIX}
      name_postfix: Candy
      # Naming method for this role
      naming_method: Firstname/Signs/Nickname
      # Custom info of overridden role
      custom_info: Just a sugar doctor
      # Is inventory overridden
      inventory_overridden: false
      # Overwritten class items list
      inventory_overwrite: []
```
