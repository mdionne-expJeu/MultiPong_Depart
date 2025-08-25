using UnityEngine;
using Unity.Netcode;

/* Script du prefab (la barre) joueur identifié comme étant le joueur (Default player prefab) dans le NetWorkManager.
Il sera automatiquement instancié (spawn) pour chaque client qui se connecte.
*/

// c'est un objet réseau (NetworkObject). Le script doit dériver de NetworkBehaviour
public class Barre : NetworkBehaviour
{
    public float vitesse; // vitesse de déplacement de la barre du joueur
    public float distanceMaxZ; // distance maximale de déplacement de la barre du joueur
    private float posClicZ; // la position du clic de la souris
    private float joueurPosZ; // la position du joueur avant le déplacement

    /* Fonction semblabe au Start, mais pour les objets réseaux. Exécuté avant le Start qui pourrait aussi 
    être utilisé. Voici l'ordre d'exécution des fonctions d'initialisation : 
    1- Awake()
    2- OnNetworkSpawn()
    3- Start()

    Le mot override indique que cette fonction est déjà présente dans un autre script pour les objets qui héritent
    du NetworkBehavior. On doit donc indiquer que c'est un override et la première ligne de la fonction fait en sorte
    que la base de cette fonction est aussi exécutée.

    On place la barre du joueur : à gauche pour l'hôte et à droite pour l'autre client */

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn(); //

        if (IsServer)
        {
            transform.position = new Vector3(-20f, 0.5f, 0f); //position à ajuster selon votre jeu
        }
        else
        {
            transform.position = new Vector3(20f, 0.5f, 0f); //position à ajuster selon votre jeu
        }
    }


    /* Dans le Update, on appelle la fonction qui gère les touches et le déplacement seulement si on est le joueur local  
   Cela permet seulement au joueur local de contrôler les déplacements de sa barre.

   Il ne faut pas oublier que le Update des 2 barres s'exécute sur le client hôte (serveur) et le client qui n'est pas serveur. 
   On ignore donc 2 cas de figure en procédant ainsi : 
   1- La barre du client qui n'est pas serveur sur l'hôte : pas d'appel de fonction;   
   2- La barre du client-hôte (serveur) sur le client qui n'est pas serveur : pas d'appel de fonction
   3- La barre du client-hôte (serveur) sur le client-hôte (serveur) : appel de fonction
   4- La barre du client qui n'est pas serveur sur le client qui n'est pas serveur : appel de fonction */
    void Update()
    {
        if (!IsLocalPlayer) return;
        GestionDeplacement();
    }


    /* Déplacement de la barre du joueur.
    Lorsque le bouton de la souris est enfoncé, on mémorise la position Y de la souris et la position z du joueur.

    Par la suite, tant que le bouton de la souris reste enfoncé :
    - On calcule la variation de position Y de la souris par rapport à la position du clic
    - On calcule la nouvelle position du joueur en fonction de la variation de position Y de la souris
    - La valeur est "clampée" pour rester dans les limites du terrain
    */
    void GestionDeplacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            posClicZ = Input.mousePosition.y;
            joueurPosZ = transform.position.z;
        }
        else if (Input.GetMouseButton(0))
        {
            float differenceZ = Input.mousePosition.y - posClicZ;
            float nouvellePositionZ = joueurPosZ + differenceZ * vitesse;
            nouvellePositionZ = Mathf.Clamp(nouvellePositionZ, -distanceMaxZ, distanceMaxZ);
            transform.position = new Vector3(transform.position.x, transform.position.y, nouvellePositionZ);
        }
    }

}
