# Astral Defense - Code Improvements

## Priority Improvements

### Already addressed
- `EnemyBehaviour` target selection is now split into smaller helper methods.
- Build flow now goes through a mediator and a placement factory.
- Credits now raise change notifications instead of staying hidden behind the build flow.

### 1. Consolidate build mode
`buildManager`, `TowerLogic`, and `Hologram` should be merged conceptually into one build system with smaller collaborators. Right now build selection, preview placement, cost checks, and final placement appear to be split in a way that will be hard to extend.
The new placement factory is a useful seam here, because it keeps the mediator from knowing prefab details directly.

### 2. Replace static economy state
`BaseScript.credits` should become part of a dedicated economy or game-state component. Static global state makes it harder to restart rounds cleanly, test the economy, or support multiple matches.

### 3. Data-drive towers and waves
Tower stats, generator bonuses, and wave configuration should move into ScriptableObjects or similar data assets. That would reduce hardcoded values and make tuning much easier.

### 4. Unify enemy spawning
`WaveManager` and `EnemySpawning` should be simplified into one explicit spawning pipeline. The game will be easier to reason about if the wave system owns cadence, counts, and spawn locations in one place.

### 5. Separate combat targeting from movement
`EnemyBehaviour` should not own all target choice, chasing, and attack decisions at once. Split those concerns into:
- target selection
- movement toward target
- attack execution

That would make the AI easier to debug and extend.

### 6. Reduce direct scene lookups
Use serialized references or a scene bootstrapper instead of repeated `GameObject.Find` calls. It will make the scene setup less brittle and reduce hidden dependencies.

## Secondary Cleanups
- Give `bullet` and other projectile scripts a shared base if they behave similarly
- Normalize naming to match Unity conventions
- Add comments around any non-obvious placement or spawn math
- Consider pooling for repeated bullets or enemies if the game spikes instantiation
