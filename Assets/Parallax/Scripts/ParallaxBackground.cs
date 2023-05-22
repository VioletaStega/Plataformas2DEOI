using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Vector2 imageSize; // Tama�o de la imagen
    public Vector2 parallaxFactor; // Factor de parallax aplicado a cada dimensi�n

    private Transform _cameraT; // Referencia a Transform de la c�mara
    private Vector3 _lastCameraPosition; // �ltima posici�n registrada de la c�mara
    private Vector3 _deltaMovement; // Desplazamiento de la c�mara con respecto a la �ltima posici�n
    private float _textureUnitSizeX; // Ancho de la imagen en unidades de Unity
    private float _textureUnitSizeY; // Alto de la imagen en unidades de Unity
    private float _offsetPositionX; // Almacena el peque�o residuo de desplazamiento en X
    private float _offsetPositionY; // Almacena el peque�o residuo de desplazamiento en Y
    private SpriteRenderer _spriteRender; // Referencia al Sprite Renderer

    void Start()
    {
        _cameraT = Camera.main.transform; // Almacena la referencia al transform de la c�mara
        _lastCameraPosition = _cameraT.position; // Registra la posici�n inicial como �ltima posici�n de la c�mara

        _spriteRender = GetComponent<SpriteRenderer>(); // Coge el componente Sprite renderer
        _spriteRender.drawMode = SpriteDrawMode.Tiled; // Pone la imagen en tipo 'Tiled'

        // Si la imagen tiene un factor distinto a 0 y a 1 entonces triplica el tama�o de la imagen en esa dimensi�n
        _spriteRender.size = new Vector2(parallaxFactor.x % 1 != 0 ? _spriteRender.size.x * 3f : _spriteRender.size.x, 
                                         parallaxFactor.y % 1 != 0 ? _spriteRender.size.y * 3f : _spriteRender.size.y);

        _textureUnitSizeX = imageSize.x / _spriteRender.sprite.pixelsPerUnit; // Calcula el ancho de la imagen en unidades Unity
        _textureUnitSizeY = imageSize.y / _spriteRender.sprite.pixelsPerUnit; // Calcula el alto de la imagen en unidades Unity
    }

    void LateUpdate()
    {
        _deltaMovement = _cameraT.position - _lastCameraPosition; // Actualiza el desplazamiento de la c�mara del �ltimo frame

        // Desplaza la imagen dependiendo del factor de parallax con respecto a la c�mara
        transform.position += new Vector3(_deltaMovement.x * parallaxFactor.x, _deltaMovement.y * parallaxFactor.y);

        _lastCameraPosition = _cameraT.position; // Registra la posici�n actual como �ltima posici�n de la c�mara

        // Si alcanza el l�mite horizontal (la diferencia de las posiciones 'x' es mayor que el ancho de la imagen)
        if (Mathf.Abs(_cameraT.position.x - transform.position.x) >= _textureUnitSizeX) 
        {
            _offsetPositionX = (_cameraT.position.x - transform.position.x) % _textureUnitSizeX; // Calcula el desplazamiento sobrante
            transform.position = new Vector3(_cameraT.position.x + _offsetPositionX, transform.position.y); // Reposiciona la imagen
        }

        // Si alcanza el l�mite vertical (la diferencia de las posiciones 'y' es mayor que el alto de la imagen)
        if (Mathf.Abs(_cameraT.position.y - transform.position.y) >= _textureUnitSizeY)
        {
            _offsetPositionY = (_cameraT.position.y - transform.position.y) % _textureUnitSizeY; // Calcula el desplazamiento sobrante
            transform.position = new Vector3(transform.position.x, _cameraT.position.y + _offsetPositionY); // Reposiciona la imagen
        }
    }
}
