using UnityEngine;

namespace BoardGame
{
    public class Pawn : MonoBehaviour
    {

        public int Id { get; private set; }

        [SerializeField] protected SpriteRenderer _sprite;

        [SerializeField] protected IPawnSelect _select;
        [SerializeField] protected IPawnMove _moveOnMap;
        [SerializeField] protected IMoveTo _initialMoveOnMap;
        [SerializeField] protected Plate _nowPlate;

        private IBoardPlayer _player;
        private PawnOwner _owner;

        protected Pawn _piggyBacking;
        protected bool _isPiggyBacked = false;

        public void SetOwner(IBoardPlayer player, PawnOwner owner, int id)
        {
            Id = id;

            _player = player;
            _owner = owner;
            _select.Initial();
        }

        public IBoardPlayer GetPlayer()
        {
            return _player;
        }

        public PawnOwner GetOwner()
        {
            return _owner;
        }

        public void SetColor(Color color)
        {
            _sprite.color = color;
        }

        public virtual Plate MovePredict(int amount)
        {

            Plate plate = _nowPlate;

            if (plate == null)
            {
                plate = GameManager.Instance.Playground.Map.GetStartPlate();
            }

            return _moveOnMap.Predict(plate, this, amount);

        }

        public virtual void Move(int amount)
        {
            OffFocus();

            void PawnMove()
            {
                _moveOnMap.MoveTo(_nowPlate, this, amount, Arrive, (value) =>
                {
                    _nowPlate = value;
                    _nowPlate.Arrive(this);
                });
            }

            if (_nowPlate != null)
            {
                PawnMove();
                return;
            }

            _owner.LeavePawn(Id);
            _nowPlate = GameManager.Instance.Playground.Map.GetStartPlate();

            _initialMoveOnMap.MoveTo(_nowPlate.transform.position, () =>
            {
                PawnMove();
            });

        }

        public void Dispose(Vector3 pos)
        {
            //if (!_owner.IsAlive()) return;
            _initialMoveOnMap.MoveTo(pos, null);
        }

        public virtual void Arrive()
        {
            if (_piggyBacking)
            {
                _piggyBacking.transform.SetParent(null);
                _piggyBacking.Arrive();
                _piggyBacking = null;
            }

            BackHome();
            GetPlayer().EndTurn();

        }

        public void BackHome()
        {

            if (_piggyBacking)
            {
                _piggyBacking.transform.SetParent(null);
                _piggyBacking.BackHome();
                _piggyBacking = null;
            }

            if (_nowPlate == null)
            {
                if (_isPiggyBacked)
                {
                    _isPiggyBacked = false;
                    GetOwner().BackHomePawn(Id);
                }
                return;
            }

            _isPiggyBacked = false;
            _nowPlate.SetPawn(null);
            _nowPlate = null;

            GetOwner().BackHomePawn(Id);

        }

        public void PiggyBack(Pawn target)
        {
            if (_piggyBacking)
            {
                _piggyBacking.PiggyBack(target);
                return;
            }
            _piggyBacking = target;
            target.PiggyBacked(this);
        }

        protected void PiggyBacked(Pawn owner)
        {
            _nowPlate = null;
            _isPiggyBacked = true;
        }

        public void EnterTurn()
        {
            if (_isPiggyBacked) return;
            _select.SetSelectable();
        }

        public void ExitTurn()
        {
            if (_isPiggyBacked) return;
            _select.SetUnselectable();
        }

        public void OnFocus()
        {
            _select.OnSelectAction();
        }

        public void OffFocus()
        {
            _select.OffSelectAction();
        }

        public bool IsPiggyBacked()
        {
            return _isPiggyBacked;
        }

        public bool IsPiggied()
        {
            return _piggyBacking != null;
        }

    }
}