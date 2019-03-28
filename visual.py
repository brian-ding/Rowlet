import pyodbc
import matplotlib.pyplot as plt
from mpl_toolkits.axisartist.parasite_axes import HostAxes, ParasiteAxes

conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=CNPC0Z76R8\\SQLEXPRESS;'
                      'Database=LJDT;'
                      'Trusted_Connection=yes;')

cursor = conn.cursor()
#cursor.execute('SELECT * FROM dbo.DealInfo')
#cursor.execute("select SUM(DealPrice)/SUM(OutArea), DealDate from LJDT.dbo.DealInfo group by DealDate order by DealDate")
#select COUNT(*), CAST(Community as varchar(max)) from LJDT.dbo.DealInfo group by CAST(Community as varchar(max)) order by COUNT(*)

# fig, ax1 = plt.subplots()
# ax2 = ax1.twinx()
# ax3 = ax1.twinx()

fig = plt.figure(1)
# main y
ax_cof = HostAxes(
    fig, [0.05, 0.05, 0.8, 0.8
          ])  #用[left, bottom, right, height]的方式定义axes，0 <= l,b,w,h <= 1
ax_cof.set_xlabel('month')
ax_cof.set_ylabel('unit price')
ax_cof.axis['bottom'].major_ticklabels.set_rotation(45)
ax_cof.axis['bottom'].major_ticklabels.set_fontsize(5)

# parasite addition axes, share x
ax_1 = ParasiteAxes(ax_cof, sharex=ax_cof)
ax_1.set_ylabel('deal count')
# ax_2 = ParasiteAxes(ax_cof, sharex=ax_cof)
# ax_2.set_ylabel('average area')

ax_cof.axis['right'].set_visible(False)
ax_cof.axis['top'].set_visible(False)
# ax_cof.set_ylim(2.8, 3.2)

# append axes
ax_cof.parasites.append(ax_1)
# ax_cof.parasites.append(ax_2)

ax1_axisline = ax_1.get_grid_helper().new_fixed_axis
# ax2_axisline = ax_2.get_grid_helper().new_fixed_axis

ax_1.axis['right2'] = ax1_axisline(loc='right', axes=ax_1, offset=(0, 0))
ax_1.axis['right2'].line.set_color('red')
ax_1.axis['right2'].label.set_color('red')
ax_1.axis['right2'].major_ticks.set_color('red')
ax_1.axis['right2'].major_ticklabels.set_color('red')

# ax_2.axis['right'] = ax2_axisline(loc='right', axes=ax_2, offset=(80, 0))
# ax_2.axis['right'].line.set_color('orange')
# ax_2.axis['right'].label.set_color('orange')
# ax_2.axis['right'].major_ticks.set_color('orange')
# ax_2.axis['right'].major_ticklabels.set_color('orange')

fig.add_axes(ax_cof)

x = []
y = []

# average DealPrice per month
cursor.execute(
    "select SUM(DealPrice)/SUM(OutArea), FORMAT(DealDate, 'yy-MM-dd') from LJDT.dbo.DealInfo group by FORMAT(DealDate, 'yy-MM-dd') order by FORMAT(DealDate, 'yy-MM-dd')"
)
x.clear()
y.clear()
for row in cursor:
    print(row)
    x.append(row[1])
    y.append(row[0])
points = ax_cof.plot(x, y, linewidth='2', label='unit price', color='green')
for a, b in zip(x, y):
    plt.text(
        a, b, "%.2f" % b, ha='center', va='top', color='green', fontsize=10)
    pass

# total deal count per month
# cursor.execute(
#     "select COUNT(*) from LJDT.dbo.DealInfo group by FORMAT(DealDate, 'yy-MM-dd') order by FORMAT(DealDate, 'yy-MM-dd')"
# )
# y.clear()
# for row in cursor:
#     y.append(row[0])
# bars = ax_1.bar(x, y, width=0.45, label='deal count', color='red')
# for b in bars:
#     h = b.get_height()
#     ax_1.text(
#         b.get_x() + b.get_width() / 2,
#         h,
#         '%d' % int(h),
#         ha='center',
#         va='bottom',
#         color='red',
#         fontsize=5)
#     pass

# average deal area per month
# cursor.execute(
#     "select SUM(OutArea)/COUNT(*) from LJDT.dbo.DealInfo group by FORMAT(DealDate, 'yy-MM-dd') order by FORMAT(DealDate, 'yy-MM-dd')"
# )
# y.clear()
# for row in cursor:
#     y.append(row[0])
# ax_2.bar(x, y, width=0.3, label='average area', color='orange')

plt.legend()
plt.show()
