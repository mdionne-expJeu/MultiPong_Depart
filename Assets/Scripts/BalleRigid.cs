using UnityEngine;
using System.Collections;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Components;// pour accéder aux propriétés du NetworkTransform


public class BalleRigid : NetworkBehaviour // objet réseau
{
    public static BalleRigid instance; // Singleton
    float maxDistanceX = 25f; // moitié de la largeur de la table, pour savoir si un but est compté
    

    // Création d'un singleton. Il ne doit y avoir qu'une seule balle.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);

    }


    /* Vérificaiton de la position de la balle pour voir si un but est compté
    - Seul le serveur fait la validation
    - Pas de validation si aucune partie en cours

    - Détection de la position de la balle. S'il y a un but :
    --- On appelle la fonction pour augmenter le score du client hote (serveur) ou du client
    --- On appelle la fonction LanceBalleMilieu qui va replacer la balle
    */
    void Update()
    {
        if (!IsServer) return;

        if (!GameManager.instance.partieEnCours) return; // Il faudra créer cette variable dans le GameManager


        //but client
        if (transform.position.x < -maxDistanceX)
        {
            ScoreManager.instance.AugmenteScoreClient();
            LanceBalleMilieu();
        }

        //but serveur (hôte)
        if (transform.position.x > maxDistanceX)
        {
            ScoreManager.instance.AugmenteHoteScore();
            LanceBalleMilieu();
        }
    }

    /* Fonction qui amorce la séquence pour replacer la balle au milieu. Fonction publique appelée aussi par
    le script GameManager.
    - Cacul du nombre de bonds à 0
    - On désactive l'interpolation du NetworkTransform pour éviter de voir l'interpolation de position de la balle
    - On replace la balle au centre de la table et on remet à 0 sa vélocité
    - Lancement d'une coroutine qui replacera et relancera la balle seulement si la partie n'est pas terminée
    */
    public void LanceBalleMilieu()
    {
        //nombreDeBonds = 0;
        GetComponent<NetworkTransform>().Interpolate = false;
        transform.position = new Vector3(0f, 0.5f, 0f);
        GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        if (GameManager.instance.partieTerminee) return; // Il faudra créer cette variable dans le GameManager
        StartCoroutine(NouvelleBalle());

    }

    /* Coroutine qui fait suite à la fonction LanceBalleMilieu()
    - Pause d'une seconde
    - On réactive l'interpolation du NetworkTransform pour des déplacements fluides côté client
    - Vélocité de la balle tirée au hasard
    */
    IEnumerator NouvelleBalle()
    {
        yield return new WaitForSecondsRealtime(1f);
       GetComponent<NetworkTransform>().Interpolate = true;

        
        float aleaX = Random.Range(0, 2) == 0 ? -10 : 10; //  opérateur ternaire
        float aleaZ = Random.Range(0, 2) == 0 ? -10 : 10;

        GetComponent<Rigidbody>().AddForce(aleaX, 0, aleaZ, ForceMode.Impulse);
    }

    /* Attention ne fonctionne pas sur le client. À utiliser seulement si détection OK seulement sur le serveur
    Fonction qui calcule le nombre de bonds de la balle et qui augmente la vitesse de cette dernière à chaque
    5 bonds.
    On appele aussi la fonction Shake du script camShaker pour faire bouger la caméra lorsque la balle frappe un mur
    ou une barre. Utilisation d'un RPC pour que tous les clients voient le shake de la caméra.
    */
    private void OnCollisionEnter(Collision infoCollisions)
    {
        
    }

    
    



    
    
}
