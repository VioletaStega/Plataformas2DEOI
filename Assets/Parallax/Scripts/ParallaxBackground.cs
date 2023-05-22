using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Vector2 imageSize; // Tamaño de la imagen
    public Vector2 parallaxFactor; // Factor de parallax aplicado a cada dimensión

    private Transform _cameraT; // Referencia a Transform de la cámara
    private Vector3 _lastCameraPosition; // Última posición registrada de la cámara
    private Vector3 _deltaMovement; // Desplazamiento de la cámara con respecto a la última posición
    private float _textureUnitSizeX; // Ancho de la imagen en unidades de Unity
    private float _textureUnitSizeY; // Alto de la imagen en unidades de Unity
    private float _offsetPositionX; // Almacena el pequeño residuo de desplazamiento en X
    private float _offsetPositionY; // Almacena el pequeño residuo de desplazamiento en Y
    private SpriteRenderer _spriteRender; // Referencia al Sprite Renderer

    void Start()
    {
        _cameraT = Camera.main.transform; // Almacena la referencia al transform de la cámara
        _lastCameraPosition = _cameraT.position; // Registra la posición inicial como última posición de la cámara

        _spriteRender = GetComponent<SpriteRenderer>(); // Coge el componente Sprite renderer
        _spriteRender.drawMode = SpriteDrawMode.Tiled; // Pone la imagen en tipo 'Tiled'

        // Si la imagen tiene un factor distinto a 0 y a 1 entonces triplica el tamaño de la imagen en esa dimensión
        _spriteRender.size = new Vector2(parallaxFactor.x % 1 != 0 ? _spriteRender.size.x * 3f : _spriteRender.size.x, 
                                         parallaxFactor.y % 1 != 0 ? _spriteRender.size.y * 3f : _spriteRender.size.y);

        _textureUnitSizeX = imageSize.x / _spriteRender.sprite.pixelsPerUnit; // Calcula el ancho de la imagen en unidades Unity
        _textureUnitSizeY = imageSize.y / _spriteRender.sprite.pixelsPerUnit; // Calcula el alto de la imagen en unidades Unity
    }

    void LateUpdate()
    {
        _deltaMovement = _cameraT.position - _lastCameraPosition; // Actualiza el desplazamiento de la cámara del último frame

        // Desplaza la imagen dependiendo del factor de parallax con respecto a la cámara
        transform.position += new Vector3(_deltaMovement.x * parallaxFactor.x, _deltaMovement.y * parallaxFactor.y);

        _lastCameraPosition = _cameraT.position; // Registra la posición actual como última posición de la cámara

        // Si alcanza el límite horizontal (la diferencia de las posiciones 'x' es mayor que el ancho de la imagen)
        if (Mathf.Abs(_cameraT.position.x - transform.position.x) >= _textureUnitSizeX) 
        {
            _offsetPositionX = (_cameraT.position.x - transform.position.x) % _textureUnitSizeX; // Calcula el desplazamiento sobrante
            transform.position = new Vector3(_cameraT.position.x + _offsetPositionX, transform.position.y); // Reposiciona la imagen
        }

        // Si alcanza el límite vertical (la diferencia de las posiciones 'y' es mayor que el alto de la imagen)
        if (Mathf.Abs(_cameraT.position.y - transform.position.y) >= _textureUnitSizeY)
        {
            _offsetPositionY = (_cameraT.position.y - transform.position.y) % _textureUnitSizeY; // Calcula el desplazamiento sobrante
            transform.position = new Vector3(transform.position.x, _cameraT.position.y + _offsetPositionY); // Reposiciona la imagen
        }
    }
}
