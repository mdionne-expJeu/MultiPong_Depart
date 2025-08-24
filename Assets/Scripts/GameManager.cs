using UnityEngine;
using Unity.Netcode; // namespace pour utiliser Netcode
using UnityEngine.SceneManagement; // namespace pour la gestion des scènes

public class GameManager : NetworkBehaviour //pour un network object
{
    public static GameManager instance;// Singleton pour parler au GameManager de n'importe où
 

    // Création du singleton si nécessaire
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fonction appelée pour le bouton qui permet de se connecter comme hôte
    public void LanceCommeHote()
    {
        NetworkManager.Singleton.StartHost();
    }

    // Fonction appelée pour le bouton qui permet de se connecter comme client
    public void LanceCommeClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    // L'hôte de la partie attend que 2 joueurs soient connectés pour lancer la partie
    // Seulement l'hôte exécute ce code
    // Aucune vérification si partie déjà en cours
    void Update()
    {
        
    }

    // Activation d'une nouvelle partie lorsque 2 joueurs. On appelle la fonction de la balle qui
    // la place au milieu et qui lui donne une vélocité.
    public void NouvellePartie()
    {
        
    }

   // Fonction appelée par le ScoreManager pour terminer la partie
    public void FinPartie()
    {
       
    }

  // Fonction appelée par le bouton Recommencer pour recommencer une partie
    public void Recommencer()
    {
        
    }
}
