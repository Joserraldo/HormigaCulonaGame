# TODO: Implementar habilidad de trepar muros con ítem

## Pasos a completar:

1. **Modificar PlayerAbilities.cs**
   - Cambiar ActivatePowerUp() para activar trepa en lugar de speed/scale
   - Agregar variables: canClimb (bool), climbDuration (10f), climbTimer (float), timerText (TextMeshProUGUI)
   - Agregar lógica en Update() para temporizador y UI

2. **Modificar New_CharacterController.cs**
   - Agregar referencia a PlayerAbilities
   - Agregar lógica en HandleMovement() para detectar colisión con "Climbable" y permitir movimiento Y si canClimb

3. **Probar en escena**
   - Verificar que el ítem active trepa por 10s
   - Verificar movimiento en Y contra paredes "Climbable"
   - Verificar display del timer

## Estado:
- [x] Modificar PlayerAbilities.cs
- [x] Modificar New_CharacterController.cs
- [x] Probar funcionalidad (ajustado raycast y estabilidad en cima)
